// Task Dashboard JavaScript
class TaskDashboard {
    constructor() {
        this.currentPage = 1;
        this.pageSize = 10;
        this.currentView = 'list';
        this.currentFilters = {
            status: '',
            priority: '',
            searchTerm: '',
            sortBy: 'dueDate'
        };

        this.init();
    }

    init() {
        this.bindEvents();
        this.loadTasks();
    }

    bindEvents() {
        // Filter form events
        $('#statusFilter, #priorityFilter').on('change', () => this.handleFilterChange());
        $('#searchBtn').on('click', () => this.handleSearch());
        $('#searchInput').on('keypress', (e) => {
            if (e.which === 13) this.handleSearch();
        });
        $('#clearFiltersBtn').on('click', () => this.clearFilters());

        // Sort and view events
        $('#sortBy').on('change', () => this.handleSortChange());
        $('#gridViewBtn').on('click', () => this.switchView('grid'));
        $('#listViewBtn').on('click', () => this.switchView('list'));

        // Pagination events will be bound dynamically
    }

    handleFilterChange() {
        this.currentFilters.status = $('#statusFilter').val();
        this.currentFilters.priority = $('#priorityFilter').val();
        this.currentPage = 1;
        this.loadTasks();
    }

    handleSearch() {
        this.currentFilters.searchTerm = $('#searchInput').val();
        this.currentPage = 1;
        this.loadTasks();
    }

    handleSortChange() {
        this.currentFilters.sortBy = $('#sortBy').val();
        this.loadTasks();
    }

    clearFilters() {
        $('#statusFilter, #priorityFilter').val('');
        $('#searchInput').val('');
        $('#sortBy').val('dueDate');
        this.currentFilters = {
            status: '',
            priority: '',
            searchTerm: '',
            sortBy: 'dueDate'
        };
        this.currentPage = 1;
        this.loadTasks();
    }

    switchView(view) {
        this.currentView = view;
        $('#gridViewBtn, #listViewBtn').removeClass('active');
        $(`#${view}ViewBtn`).addClass('active');

        if (view === 'grid') {
            $('#listView').hide();
            $('#gridView').show();
        } else {
            $('#gridView').hide();
            $('#listView').show();
        }

        this.loadTasks();
    }

    showLoading() {
        $('#loadingIndicator').show();
        $('#tasksGridContainer, #tasksListContainer').empty();
        $('#tasksPagination, #noTasksMessage').hide();
    }

    hideLoading() {
        $('#loadingIndicator').hide();
    }

    async loadTasks() {
        this.showLoading();

        try {
            const params = new URLSearchParams({
                page: this.currentPage,
                pageSize: this.pageSize,
                status: this.currentFilters.status,
                priority: this.currentFilters.priority,
                searchTerm: this.currentFilters.searchTerm,
                sortBy: this.currentFilters.sortBy
            });

            const response = await fetch(`/GetTasks?${params}`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                }
            });

            const result = await response.json();

            if (result.success) {
                this.renderTasks(result.data);
                this.renderPagination(result.totalPages, result.currentPage);

                if (result.data.length === 0) {
                    $('#noTasksMessage').show();
                    $('#tasksPagination').hide();
                } else {
                    $('#noTasksMessage').hide();
                    if (result.totalPages > 1) {
                        $('#tasksPagination').show();
                    }
                }
            } else {
                console.error('Error loading tasks:', result.message);
                this.showError('Failed to load tasks. Please try again.');
            }
        } catch (error) {
            console.error('Error loading tasks:', error);
            this.showError('Failed to load tasks. Please check your connection and try again.');
        } finally {
            this.hideLoading();
        }
    }

    renderTasks(tasks) {
        const container = this.currentView === 'grid' ? '#tasksGridContainer' : '#tasksListContainer';
        $(container).empty();

        if (tasks.length === 0) {
            return;
        }

        tasks.forEach(task => {
            const taskCard = this.createTaskCard(task);
            if (this.currentView === 'grid') {
                $(container).append(`<div class="col-lg-4 col-md-6 mb-4">${taskCard}</div>`);
            } else {
                $(container).append(`<div class="mb-3">${taskCard}</div>`);
            }
        });
    }

    createTaskCard(task) {
        const statusBadge = this.getStatusBadge(task.taskStatus);
        const priorityBadge = this.getPriorityBadge(task.taskPriority);
        const dueDateInfo = this.getDueDateInfo(task.dueDate, task.taskStatus);
        const isOverdue = new Date(task.dueDate) < new Date() && task.taskStatus !== 'Completed';

        return `
            <div class="card h-100 ${isOverdue ? 'border-danger' : ''}">
                <div class="card-header d-flex justify-content-between align-items-start">
                    <div class="flex-grow-1">
                        <h6 class="card-title mb-1 ${isOverdue ? 'text-danger' : ''}">${task.title}</h6>
                        <small class="text-muted">Created by ${task.userName}</small>
                    </div>
                    <div class="dropdown">
                        <button class="btn btn-sm btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                        </button>
                        <ul class="dropdown-menu">
                            <li><a class="dropdown-item" href="#" onclick="taskDashboard.viewTask(${task.id})">
                                <i class="fas fa-eye me-2"></i>View Details
                            </a></li>
                            <li><a class="dropdown-item" href="#" onclick="taskDashboard.editTask(${task.id})">
                                <i class="fas fa-edit me-2"></i>Edit
                            </a></li>
                            <li><hr class="dropdown-divider"></li>
                            <li><a class="dropdown-item text-danger" href="#" onclick="taskDashboard.deleteTask(${task.id})">
                                <i class="fas fa-trash me-2"></i>Delete
                            </a></li>
                        </ul>
                    </div>
                </div>
                <div class="card-body">
                    <p class="card-text text-muted small mb-3">${task.description}</p>
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <div>
                            ${statusBadge}
                            ${priorityBadge}
                        </div>
                    </div>
                    <div class="d-flex justify-content-between align-items-center">
                        <small class="text-muted">
                            <i class="fas fa-clock me-1"></i>
                            ${dueDateInfo}
                        </small>
                        <div class="btn-group btn-group-sm" role="group">
                            ${this.getStatusButtons(task)}
                        </div>
                    </div>
                </div>
            </div>`;
    }

    getStatusBadge(status) {
        const statusClasses = {
            'Pending': 'bg-warning text-dark',
            'In Progress': 'bg-info text-white',
            'Completed': 'bg-success text-white',
            'On Hold': 'bg-secondary text-white'
        };

        const className = statusClasses[status] || 'bg-secondary text-white';
        return `<span class="badge ${className} me-2">${status}</span>`;
    }

    getPriorityBadge(priority) {
        const priorityClasses = {
            'Low': 'bg-light text-dark',
            'Medium': 'bg-primary text-white',
            'High': 'bg-warning text-dark',
            'Critical': 'bg-danger text-white'
        };

        const className = priorityClasses[priority] || 'bg-secondary text-white';
        return `<span class="badge ${className}">${priority}</span>`;
    }

    getDueDateInfo(dueDate, status) {
        const due = new Date(dueDate);
        const now = new Date();
        const isOverdue = due < now && status !== 'Completed';

        const options = {
            year: 'numeric',
            month: 'short',
            day: 'numeric'
        };

        const formattedDate = due.toLocaleDateString('en-US', options);

        if (isOverdue) {
            return `<span class="text-danger">Overdue: ${formattedDate}</span>`;
        } else if (status === 'Completed') {
            return `Completed: ${formattedDate}`;
        } else {
            return `Due: ${formattedDate}`;
        }
    }

    getStatusButtons(task) {
        const buttons = [];

        if (task.taskStatus === 'Pending') {
            buttons.push(`<button class="btn btn-outline-info btn-sm" onclick="taskDashboard.updateTaskStatus(${task.id}, 'In Progress')">Start</button>`);
        }

        if (task.taskStatus === 'In Progress') {
            buttons.push(`<button class="btn btn-outline-success btn-sm" onclick="taskDashboard.updateTaskStatus(${task.id}, 'Completed')">Complete</button>`);
        }

        if (task.taskStatus !== 'Completed') {
            buttons.push(`<button class="btn btn-outline-warning btn-sm" onclick="taskDashboard.updateTaskStatus(${task.id}, 'On Hold')">Hold</button>`);
        }

        return buttons.join('');
    }

    renderPagination(totalPages, currentPage) {
        if (totalPages <= 1) {
            $('#tasksPagination').hide();
            return;
        }

        const paginationHtml = [];

        // Previous button
        paginationHtml.push(`
            <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
                <a class="page-link" href="#" onclick="taskDashboard.changePage(${currentPage - 1})">Previous</a>
            </li>
        `);

        // Page numbers
        for (let i = 1; i <= totalPages; i++) {
            if (i === 1 || i === totalPages || (i >= currentPage - 2 && i <= currentPage + 2)) {
                paginationHtml.push(`
                    <li class="page-item ${i === currentPage ? 'active' : ''}">
                        <a class="page-link" href="#" onclick="taskDashboard.changePage(${i})">${i}</a>
                    </li>
                `);
            } else if (i === currentPage - 3 || i === currentPage + 3) {
                paginationHtml.push('<li class="page-item disabled"><span class="page-link">...</span></li>');
            }
        }

        // Next button
        paginationHtml.push(`
            <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
                <a class="page-link" href="#" onclick="taskDashboard.changePage(${currentPage + 1})">Next</a>
            </li>
        `);

        $('#paginationList').html(paginationHtml.join(''));
        $('#tasksPagination').show();
    }

    changePage(page) {
        if (page >= 1 && page !== this.currentPage) {
            this.currentPage = page;
            this.loadTasks();
        }
    }

    async updateTaskStatus(taskId, newStatus) {
        try {
            const response = await fetch(`/UpdateStatus/${taskId}`, {
                method: 'PATCH',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                },
                body: JSON.stringify(newStatus)
            });

            const result = await response.json();

            if (result.success) {
                this.showSuccess('Task status updated successfully!');
                this.loadTasks(); // Reload tasks to reflect changes
            } else {
                this.showError('Failed to update task status.');
            }
        } catch (error) {
            console.error('Error updating task status:', error);
            this.showError('Failed to update task status.');
        }
    }

    async deleteTask(taskId) {
        if (!confirm('Are you sure you want to delete this task?')) {
            return;
        }

        try {
            const response = await fetch(`/Delete/${taskId}`, {
                method: 'DELETE',
                headers: {
                    'RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                }
            });

            const result = await response.json();

            if (result.success) {
                this.showSuccess('Task deleted successfully!');
                this.loadTasks(); // Reload tasks to reflect changes
            } else {
                this.showError('Failed to delete task.');
            }
        } catch (error) {
            console.error('Error deleting task:', error);
            this.showError('Failed to delete task.');
        }
    }

    viewTask(taskId) {
        // Implement view task functionality
        console.log('View task:', taskId);
        // You could open a modal or navigate to a details page
    }

    editTask(taskId) {
        // Implement edit task functionality
        console.log('Edit task:', taskId);
        // You could open a modal or navigate to an edit page
    }

    showSuccess(message) {
        this.showToast(message, 'success');
    }

    showError(message) {
        this.showToast(message, 'danger');
    }

    showToast(message, type) {
        const toastHtml = `
            <div class="toast align-items-center text-white bg-${type} border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">${message}</div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;

        // Create toast container if it doesn't exist
        if (!$('#toastContainer').length) {
            $('body').append('<div id="toastContainer" class="toast-container position-fixed top-0 end-0 p-3"></div>');
        }

        const $toast = $(toastHtml);
        $('#toastContainer').append($toast);

        const toast = new bootstrap.Toast($toast[0]);
        toast.show();

        // Remove toast element after it's hidden
        $toast.on('hidden.bs.toast', function () {
            $(this).remove();
        });
    }
}

// Initialize dashboard when document is ready
$(document).ready(function () {
    window.taskDashboard = new TaskDashboard();
});

// Handle page refresh/navigation
$(window).on('beforeunload', function () {
    // Cleanup if needed
});