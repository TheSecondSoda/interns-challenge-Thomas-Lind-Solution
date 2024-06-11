using System;
using System.Collections;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HolidayCalendar;
public class HolidayCalendar : IHolidayCalendar
{
	private static HttpClient _httpClient = new()
	{
		BaseAddress = new Uri("https://api.sallinggroup.com"),
		DefaultRequestHeaders = { Authorization = new AuthenticationHeaderValue("Bearer", "e99e6a68-e717-4245-b1e4-790011fc09c7") }
	};

  public bool IsHoliday(DateTime date)
  {
	  //Send request
	  var task = _httpClient.GetFromJsonAsync<bool>($"/v1/holidays/is-holiday?date={date:yyyy-MM-dd}");
	  task.Wait();

	  return task.Result;
  }

  public ICollection<DateTime> GetHolidays(DateTime startDate, DateTime endDate)
  {
	  //Send request
	  var task = _httpClient.GetFromJsonAsync<List<Holiday>>(
		  $"/v1/holidays?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
	  task.Wait();
		
	  //Make result list
	  var result = new List<DateTime>();
	  //Add dates for holidays to result
	  foreach (var holiday in task.Result.Where(h => h.nationalHoliday == true))
	  {
		  result.Add(holiday.date);
	  }

	  return result;
  }
}
