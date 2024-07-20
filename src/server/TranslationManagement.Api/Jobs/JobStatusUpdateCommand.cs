namespace TranslationManagement.Api.Jobs;

public record JobStatusUpdateCommand(int JobId, JobStatus NewStatus);