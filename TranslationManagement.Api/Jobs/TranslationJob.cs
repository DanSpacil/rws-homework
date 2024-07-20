using TranslationManagement.Api.Workflow;

namespace TranslationManagement.Api.Jobs;

public class TranslationJob
{
    public int Id { get; set; }
    public string CustomerName { get; set; }

    public JobStatus Status { get; set; }
    public string OriginalContent { get; set; }
    public string TranslatedContent { get; set; }
    public double Price { get; private set; }

    public void SetPrice(double pricePerCharacter)
    {
        Price = OriginalContent.Length * pricePerCharacter;
    }

    public Result<bool> UpdateStatus(JobStatus newStatus)
    {
        if ((Status == JobStatus.New && newStatus == JobStatus.Completed) ||
            Status == JobStatus.Completed ||
            newStatus == JobStatus.New)
        {
            return Result<bool>.Error("Invalid status transition");
        }

        Status = newStatus;
        return Result<bool>.Success(true);
    }
}