using FluentAssertions;
using Moq;
using TranslationManagement.Api.Jobs;
using TranslationManagement.Api.Jobs.Persistence;
using TranslationManagement.Api.Notifications;
using TranslationManagement.Api.Persistence;

namespace TranslationManagement.Unit.Tests.Jobs;

public class TranslationJobServiceTest
{
    private readonly TranslationJobService _sut;
    private readonly Mock<ITranslationJobRepository> _repositoryMock;

    public TranslationJobServiceTest()
    {
        _repositoryMock = new Mock<ITranslationJobRepository>();
        var notifierMock = new Mock<INotifier>();
        _sut = new TranslationJobService(notifierMock.Object,_repositoryMock.Object );
    }

    [Fact]
    public async Task UpdateJobStatus_InvalidJob_ReturnsErrorResult()
    {
       // Arrange
       var updateCommand = new JobStatusUpdateCommand(1, JobStatus.New);
       _repositoryMock.Setup(m => m.GetJobById(It.IsAny<int>()))
           .ReturnsAsync((int _) => null);
       
       //Act
       var updateResult = await _sut.UpdateJobStatus(updateCommand);

       //Assert
       updateResult.IsUpdated.Should().BeFalse();
    }
    
    [Fact]
    public async Task UpdateJobStatus_ValidJob_UpdatesStatus()
    {
       // Arrange
       var updateCommand = new JobStatusUpdateCommand(1, JobStatus.InProgress);
       var translationJob = new TranslationJob(){ Status = JobStatus.New };
       _repositoryMock.Setup(m => m.GetJobById(It.IsAny<int>()))
           .ReturnsAsync((int _) => translationJob);
       
       //Act
       var updateResult = await _sut.UpdateJobStatus(updateCommand);

       //Assert
       updateResult.IsUpdated.Should().BeTrue();
       translationJob.Status.Should().Be(JobStatus.InProgress);
    }
}