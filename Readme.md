# Test Task Docker Instructions

## Build the Docker Image


docker build -t testtask .

## Run the Docker container
docker run -p 55005:8080 testtask:latest


# General notes

## The idea behind the solution is:
 * Top posts are probably not getting modified/changed to often
 	* So that's potentially a read heavy operation
 * I would expect that data consistency is not playing a crusial role here
	* E.g. we don't need to show any top post changes right away and can afford caching...
 * Taking into account the assumptions above it makes sense to build a cache of top posts and update it from the background worker.
	Benefits:
	1. Requests to our API are completely decoupled from requests to Hackerrank api
	2. That means if we're getting under pressure - we're not starting to issue a top of requests to 3rd party service
	3. That also means our response time is going to be prett low (we're doing good when it comes to response time/performance)
	Drawbacks
	1. We're not showing the most relevant data
	2. The approach is not applicable for write heavy applications or applications that require high data consistence (in the meaning of always being up to date with the data source)

## Solution structure
The idea is to have a common set of contracts for the task in TestTask.PostingsClient.Contracts
Then all the HackerRankC/Http specific goes into TestTask.HackerRankClient
Note: we could split the Http/caching/cache updates part from the hackerrank specific.
I didn't do it due to the following judgement:
1. The source of data is not necessarily a 3rd party system and needs that specifics
2. We only have 1 Http source now, so we could split it in case we would add additional "Stackoverflow" client

	

# Notes/Improvements:

## Requests throttling
Throttler could be added (Gubernatr could do the trick). Though it's not an absolute must (taking into account we are doing requests in backgroung and have a full control over it)
HackerRank client specifics could be separated from the cache/updates part.
That way cache/updates would be reusable.
Note: 
	1. haven't done that cause it might make no sense at this point and would be a bit of an overengineering 
	2. Tt would make sense if we had 2+ 3rd party data sources
	3. E.g. db could also act as data source for us and wouldn't require overcomplicating things
Retry policies on HttpClients could be added (potentially with a backoff or circuit breaker patterns)



## Testing
This is an important part, but:
 * since I've already spent ~2h on the assignment
 * since that is pretty much a CRUD
I'm leaving it aside (though it makes sense testing cache behaviour)