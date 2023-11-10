# Результаты нагрузочного тестирование

## 1 бэкенд
```
This is ApacheBench, Version 2.3 <$Revision: 1903618 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking localhost (be patient)
Completed 100 requests
Completed 200 requests
Completed 300 requests
Completed 400 requests
Completed 500 requests
Finished 500 requests


Server Software:        Kestrel
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1/zones
Document Length:        1887 bytes

Concurrency Level:      10
Time taken for tests:   3.884 seconds
Complete requests:      500
Failed requests:        0
Total transferred:      1032000 bytes
HTML transferred:       943500 bytes
Requests per second:    128.74 [#/sec] (mean)
Time per request:       77.677 [ms] (mean)
Time per request:       7.768 [ms] (mean, across all concurrent requests)
Transfer rate:          259.49 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.6      1       7
Processing:    34   76  15.6     75     157
Waiting:       29   75  15.6     74     156
Total:         34   76  15.6     75     157
WARNING: The median and mean for the initial connection time are not within a normal deviation
        These results are probably not that reliable.

Percentage of the requests served within a certain time (ms)
  50%     75
  66%     82
  75%     86
  80%     88
  90%     95
  95%    103
  98%    111
  99%    121
 100%    157 (longest request)
```

Подробности выполнения всех запросов [здесь](./1-ins-nginx.data).

### 3 бэкенд по распределению 2:1:1
```
Server Software:        Kestrel
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1/zones
Document Length:        1887 bytes

Concurrency Level:      10
Time taken for tests:   4.023 seconds
Complete requests:      500
Failed requests:        0
Total transferred:      1032000 bytes
HTML transferred:       943500 bytes
Requests per second:    124.27 [#/sec] (mean)
Time per request:       80.467 [ms] (mean)
Time per request:       8.047 [ms] (mean, across all concurrent requests)
Transfer rate:          250.49 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.4      1       4
Processing:    37   79  17.9     76     226
Waiting:       31   78  17.9     75     225
Total:         37   79  17.9     77     227
ERROR: The median and mean for the initial connection time are more than twice the standard
       deviation apart. These results are NOT reliable.

Percentage of the requests served within a certain time (ms)
  50%     77
  66%     83
  75%     87
  80%     90
  90%     98
  95%    108
  98%    128
  99%    142
 100%    227 (longest request)
```

Подробности выполнения всех запросов [здесь](./3-ins-nginx.data).
