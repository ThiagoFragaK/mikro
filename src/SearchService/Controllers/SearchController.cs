namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<Item>> SearchItems([FromQuery]SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item, Item>();
        if(!string.IsNullOrEmpty(searchParamssearchTerm))
        {
            query.Match(SearchController.Full, searchParams.searchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch 
        {
            "make" => query.Sort(x => x.Ascending(r => r.Make)),
            "new" => query.Sort(x => x.Descending(r => r.CreatedAt)),
            _ => query.Sort(x => x.Ascending(r => r.AuctionEnd))
        };

        query = searchParams.FilterBy switch
        {
            "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
            "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHour(6) && x.AuctionEnd > DateTime.UtcNow),
            _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)
        };

        query.PageNumber(searchParams.pageNumber);
        query.PageSize(searchParams.pageSize);

        var result = await query.ExecuteAsync();
        return Ok(new {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }

    
}