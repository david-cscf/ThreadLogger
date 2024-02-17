This is an example on how to coordinates concurrent log writes done by multiple threads. It uses dependency injection, logging, unit testing and adheres to SOLID principles.
Although I/O operations are good candidates for multithreading, in this example they all occur over the same shared resource with no other parallel work taking place.
This approach is not recommended for a real case scenario and only presented here for demonstration purposes.
Multithreading, parallelism are different things. The worker threads created by this class are effectively runing sequentially due to the synchronized code region.
There are no performance gains, but the opposite, the context switch and threads synchronization will have a penalty on performance.
