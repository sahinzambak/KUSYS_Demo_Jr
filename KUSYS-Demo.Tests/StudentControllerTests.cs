using KUSYS_Demo.Controllers;
using KUSYS_Demo.Data;
using KUSYS_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace KUSYS_Demo.Test
{
    public class StudentControllerTests
    {
        private DbContextOptions<KUSYSDbContext> _dbContextOptions;

        public StudentControllerTests()
        {
            // Use in-memory database for testing
            _dbContextOptions = new DbContextOptionsBuilder<KUSYSDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public void SaveNewStudent_ValidStudent_ReturnsJsonResultWithSuccess()
        {
            // Arrange
            var student = new Student
            {
                FirstName = "Ali",
                LastName = "Cin",
                BirthDate = new DateTime(1995, 10, 15),
                CourseId = "CSI101"
            };

            // Act
            int result;
            using (var dbContext = new KUSYSDbContext(_dbContextOptions))
            {
                var controller = new StudentController(dbContext);
                var jsonResult = controller.SaveNewStudent(student) as JsonResult;
                result = (int)jsonResult.Value;
            }

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void EditStudent_ExistingStudent_ReturnsJsonResultWithSuccess()
        {
            // Arrange
            var existingStudent = new Student
            {
                StudentId = 1,
                FirstName = "Koray",
                LastName = "Ürkmez",
                BirthDate = new DateTime(1995, 10, 15),
                CourseId = "CSI101"
            };

            using (var dbContext = new KUSYSDbContext(_dbContextOptions))
            {
                dbContext.Students.Add(existingStudent);
                dbContext.SaveChanges();
            }

            // Updated student data
            var updatedStudent = new Student
            {
                StudentId = existingStudent.StudentId,
                FirstName = "Ökkeþ",
                LastName = "Karabulut",
                BirthDate = new DateTime(1990, 5, 20),
                CourseId = "MAT101"
            };

            // Act
            int result;
            using (var dbContext = new KUSYSDbContext(_dbContextOptions))
            {
                var controller = new StudentController(dbContext);
                var jsonResult = controller.EditStudent(updatedStudent) as JsonResult;
                result = (int)jsonResult.Value;
            }

            // Assert
            Assert.Equal(1, result);

            using (var dbContext = new KUSYSDbContext(_dbContextOptions))
            {
                var editedStudent = dbContext.Students.Find(existingStudent.StudentId);
                Assert.NotNull(editedStudent);
                Assert.Equal(updatedStudent.FirstName, editedStudent.FirstName);
                Assert.Equal(updatedStudent.LastName, editedStudent.LastName);
                Assert.Equal(updatedStudent.BirthDate, editedStudent.BirthDate);
                Assert.Equal(updatedStudent.CourseId, editedStudent.CourseId);
            }
        }

        [Fact]
        public void DeleteStudent_ExistingStudent_ReturnsJsonResultWithSuccess()
        {
            // Arrange
            var existingStudent = new Student
            {
                StudentId = 1,
                FirstName = "Chester",
                LastName = "Benington",
                BirthDate = new DateTime(1995, 10, 15),
                CourseId = "CSI101"
            };

            using (var dbContext = new KUSYSDbContext(_dbContextOptions))
            {
                dbContext.Students.Add(existingStudent);
                dbContext.SaveChanges();
            }

            // Act
            int result;
            using (var dbContext = new KUSYSDbContext(_dbContextOptions))
            {
                var controller = new StudentController(dbContext);
                var jsonResult = controller.DeleteStudent(existingStudent.StudentId) as JsonResult;
                result = (int)jsonResult.Value;
            }

            // Assert
            Assert.Equal(1, result);

            using (var dbContext = new KUSYSDbContext(_dbContextOptions))
            {
                var deletedStudent = dbContext.Students.Find(existingStudent.StudentId);
                Assert.Null(deletedStudent);
            }
        }
    }
}
