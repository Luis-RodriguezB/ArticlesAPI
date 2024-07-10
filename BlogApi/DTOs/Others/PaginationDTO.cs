namespace ArticlesAPI.DTOs.Others;
public class PaginationDTO
{
    public int Page { get; set; } = 1;
    private int recordsByPage = 25;
    private readonly int maxRecordsByPage = 50;

    public int RecordsByPage
    {
        get { return recordsByPage; }
        set
        {
            recordsByPage = value > maxRecordsByPage ? maxRecordsByPage : value;
        }
    }
}
