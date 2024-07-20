using FluentAssertions;
using TranslationManagement.Api.Jobs;

namespace TranslationManagement.Unit.Tests.Jobs;

public class TranslationJobTest
{
    [Fact]
    public void NewJob_ChangeToInProgress_IsAllowed()
    {
        // Arrange
        var translationJob = new TranslationJob() { Status = JobStatus.New };

        // Act
        var updateResult = translationJob.UpdateStatus(JobStatus.InProgress);

        // Assert
        updateResult.IsSuccess.Should().BeTrue();
        translationJob.Status.Should().Be(JobStatus.InProgress);
    }

    [Theory]
    [InlineData(JobStatus.InProgress)]
    [InlineData(JobStatus.New)]
    public void CompletedJob_StatusChange_IsNotAllowed(JobStatus newStatus)
    {
        // Arrange
        var translationJob = new TranslationJob() { Status = JobStatus.Completed };

        // Act
        var updateResult = translationJob.UpdateStatus(newStatus);

        // Assert
        updateResult.IsSuccess.Should().BeFalse();
        translationJob.Status.Should().Be(JobStatus.Completed);
    }
}