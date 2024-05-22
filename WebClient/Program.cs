// using RestSharp;


// var client = new RestClient("http://localhost:5285");

// var request = new RestRequest("/api/item", Method.GET);

// var response = await client.ExecuteAsync(request);


// Console.WriteLine($"Request: {request.Resource}");
// Console.WriteLine($"Result: {response.Content}");
// Console.WriteLine($"Request Status: {response.ResponseStatus}");
// Console.WriteLine($"Error Exception: {response.ErrorException?.Message}");
// Console.WriteLine($"Error Message: {response.ErrorMessage}");
// Console.WriteLine($"Succuss: {response.IsSuccessful}");

// Console.WriteLine($"==========================================");

// var failedRequest = new RestRequest("/api/item/fail", Method.GET);
// response = await client.ExecuteAsync(failedRequest);

// Console.WriteLine($"Request: {failedRequest.Resource}");
// Console.WriteLine($"Result: {response.Content}");
// Console.WriteLine($"Request Status: {response.ResponseStatus}");
// Console.WriteLine($"Error Exception: {response.ErrorException?.Message}");
// Console.WriteLine($"Error Message: {response.ErrorMessage}");
// Console.WriteLine($"Succuss: {response.IsSuccessful}");


// Console.WriteLine($"==========================================");

// var notFoundRequest = new RestRequest("/api/item/123", Method.GET);
// response = await client.ExecuteAsync(notFoundRequest);

// Console.WriteLine($"Request: {notFoundRequest.Resource}");
// Console.WriteLine($"Result: {response.Content}");
// Console.WriteLine($"Request Status: {response.ResponseStatus}");
// Console.WriteLine($"Error Exception: {response.ErrorException?.Message}");
// Console.WriteLine($"Error Message: {response.ErrorMessage}");
// Console.WriteLine($"Succuss: {response.IsSuccessful}");

// Console.WriteLine($"==========================================");


// var errorRequest = new RestRequest("/api/item/error", Method.GET);
// response = await client.ExecuteAsync(errorRequest);

// Console.WriteLine($"Request: {errorRequest.Resource}");
// Console.WriteLine($"Result: {response.Content}");
// Console.WriteLine($"Request Status: {response.ResponseStatus}");
// Console.WriteLine($"Error Exception: {response.ErrorException?.Message}");
// Console.WriteLine($"Error Message: {response.ErrorMessage}");
// Console.WriteLine($"Succuss: {response.IsSuccessful}");

// using System.Text.RegularExpressions;

// string pattern = @"^310\d\d";

// Console.WriteLine(Regex.IsMatch("31011", pattern));

