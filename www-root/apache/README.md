# Результаты нагрузочного тестирование

## Бэкенд приложение на 8080 порту
```
Server Software:        Kestrel
Server Hostname:        localhost
Server Port:            8080

Document Path:          /api/v1/zones
Document Length:        2899 bytes

Concurrency Level:      10
Time taken for tests:   3.386 seconds
Complete requests:      1000
Failed requests:        0
Total transferred:      3060000 bytes
HTML transferred:       2899000 bytes
Requests per second:    295.35 [#/sec] (mean)
Time per request:       33.858 [ms] (mean)
Time per request:       3.386 [ms] (mean, across all concurrent requests)
Transfer rate:          882.59 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.4      0       1
Processing:    10   33   9.2     32     113
Waiting:        8   25   8.0     24     105
Total:         10   34   9.2     33     113

Percentage of the requests served within a certain time (ms)
  50%     33
  66%     35
  75%     36
  80%     37
  90%     40
  95%     42
  98%     46
  99%    108
 100%    113 (longest request)
```

Подробности выполнения всех запросов [здесь](./1-ins-8080.data).
## С использованием Nginx

### 1 бэкенд 
```
Server Software:        nginx/1.25.3
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1/zones
Document Length:        2899 bytes

Concurrency Level:      10
Time taken for tests:   3.731 seconds
Complete requests:      1000
Failed requests:        0
Total transferred:      3065000 bytes
HTML transferred:       2899000 bytes
Requests per second:    268.01 [#/sec] (mean)
Time per request:       37.313 [ms] (mean)
Time per request:       3.731 [ms] (mean, across all concurrent requests)
Transfer rate:          802.18 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.4      0       1
Processing:    12   37   7.0     37      65
Waiting:       11   36   7.0     36      63
Total:         12   37   7.0     37      65

Percentage of the requests served within a certain time (ms)
  50%     37
  66%     40
  75%     41
  80%     43
  90%     46
  95%     49
  98%     53
  99%     56
 100%     65 (longest request)
```

Подробности выполнения всех запросов [здесь](./1-ins-nginx.data).

### 3 бэкенд по распределению 2:1:1
```
Server Software:        nginx/1.25.3
Server Hostname:        localhost
Server Port:            80

Document Path:          /api/v1/zones
Document Length:        2899 bytes

Concurrency Level:      10
Time taken for tests:   5.387 seconds
Complete requests:      1000
Failed requests:        252
   (Connect: 0, Receive: 0, Length: 252, Exceptions: 0)
Non-2xx responses:      252
Total transferred:      2372252 bytes
HTML transferred:       2201716 bytes
Requests per second:    185.65 [#/sec] (mean)
Time per request:       53.866 [ms] (mean)
Time per request:       5.387 [ms] (mean, across all concurrent requests)
Transfer rate:          430.08 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.4      0       1
Processing:    10   53  61.7     27     523
Waiting:        8   53  61.7     26     522
Total:         10   54  61.7     27     523

Percentage of the requests served within a certain time (ms)
  50%     27
  66%     39
  75%     73
  80%     94
  90%    129
  95%    155
  98%    189
  99%    402
 100%    523 (longest request)
```

Подробности выполнения всех запросов [здесь](./3-ins-nginx.data).
