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

    public JobStatusUpdateResult UpdateStatus(JobStatus newStatus)
    {
        if ((this.Status == JobStatus.New && newStatus == JobStatus.Completed) ||
            this.Status == JobStatus.Completed ||
            newStatus == JobStatus.New)
        {
            return JobStatusUpdateResult.Invalid();
        }

        this.Status = newStatus;
        return JobStatusUpdateResult.Success();
    }
}