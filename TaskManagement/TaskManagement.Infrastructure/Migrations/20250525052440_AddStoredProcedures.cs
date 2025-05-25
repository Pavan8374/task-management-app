using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    IF OBJECT_ID('sp_CreateTask', 'P') IS NOT NULL
                        DROP PROCEDURE sp_CreateTask;
                ");
            migrationBuilder.Sql(@"
                    CREATE PROCEDURE sp_CreateTask
                        @Title NVARCHAR(255),
                        @Description NVARCHAR(MAX),
                        @TaskStatus NVARCHAR(100),
                        @TaskPriority NVARCHAR(100),
                        @DueDate DATETIME,
                        @UserId INT
                    AS
                    BEGIN
                        INSERT INTO Tasks (
                            Title, Description, TaskStatus, TaskPriority, DueDate, UserId, IsDeleted, IsActive, CreatedAt
                        )
                        VALUES (
                            @Title, @Description, @TaskStatus, @TaskPriority, @DueDate, @UserId, 0, 1, GETDATE()
                        );
                    END;
            ");

            migrationBuilder.Sql("IF OBJECT_ID('sp_UpdateTask', 'P') IS NOT NULL DROP PROCEDURE sp_UpdateTask;");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_UpdateTask
                    @Id INT,
                    @Title NVARCHAR(255),
                    @Description NVARCHAR(MAX),
                    @TaskStatus NVARCHAR(100),
                    @TaskPriority NVARCHAR(100),
                    @DueDate DATETIME
                AS
                BEGIN
                    UPDATE Tasks
                    SET 
                        Title = @Title,
                        Description = @Description,
                        TaskStatus = @TaskStatus,
                        TaskPriority = @TaskPriority,
                        DueDate = @DueDate,
                        ModifiedAt = GETDATE()
                    WHERE Id = @Id AND IsDeleted = 0;
                END;
            ");

            migrationBuilder.Sql("IF OBJECT_ID('sp_DeleteTask', 'P') IS NOT NULL DROP PROCEDURE sp_DeleteTask;");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_DeleteTask
                    @Id INT
                AS
                BEGIN
                    UPDATE Tasks
                    SET 
                        IsDeleted = 1,
                        ModifiedAt = GETDATE()
                    WHERE Id = @Id;
                END;
            ");

            migrationBuilder.Sql("IF OBJECT_ID('sp_UpdateTaskStatus', 'P') IS NOT NULL DROP PROCEDURE sp_UpdateTaskStatus;");
            migrationBuilder.Sql(@"
                CREATE PROCEDURE sp_UpdateTaskStatus
                    @Id INT,
                    @TaskStatus NVARCHAR(100)
                AS
                BEGIN
                    UPDATE Tasks
                    SET 
                        TaskStatus = @TaskStatus,
                        ModifiedAt = GETDATE()
                    WHERE Id = @Id AND IsDeleted = 0;
                END;
            ");

            migrationBuilder.Sql("IF OBJECT_ID('sp_UpdateTaskPriority', 'P') IS NOT NULL DROP PROCEDURE sp_UpdateTaskPriority;");
            migrationBuilder.Sql(@"
                    CREATE PROCEDURE sp_UpdateTaskPriority
                        @Id INT,
                        @TaskPriority NVARCHAR(100)
                    AS
                    BEGIN
                        UPDATE Tasks
                        SET 
                            TaskPriority = @TaskPriority,
                            ModifiedAt = GETDATE()
                        WHERE Id = @Id AND IsDeleted = 0;
                    END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_CreateTask;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_UpdateTask;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_DeleteTask;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_UpdateTaskStatus;");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS sp_UpdateTaskPriority;");
        }
    }
}
