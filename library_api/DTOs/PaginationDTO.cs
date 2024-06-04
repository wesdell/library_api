namespace library_api.DTOs
{
	public class PaginationDTO
	{
		public int Page { get; set; } = 1;
		private int RecordsToShowPerPage = 10;
		private readonly int MaximumRecordsPerPage = 20;

		public int RecordsPerPage
		{
			get
			{
				return this.RecordsToShowPerPage;
			}

			set
			{
				this.RecordsToShowPerPage = (value > MaximumRecordsPerPage) ? this.MaximumRecordsPerPage : value;
			}
		}
	}
}