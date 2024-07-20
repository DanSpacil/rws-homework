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
        this.Price = this.OriginalContent.Length * pricePerCharacter;
    }

    public Result<bool> UpdateStatus(JobStatus newStatus)
    {
        if ((this.Status == JobStatus.New && newStatus == JobStatus.Completed) ||
            this.Status == JobStatus.Completed ||
            newStatus == JobStatus.New)
        {
            return Result<bool>.Error("Invalid status transition");
        }

        this.Status = newStatus;
        return Result<bool>.Success(true);
    }
}