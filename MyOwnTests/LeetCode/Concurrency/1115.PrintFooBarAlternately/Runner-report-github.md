```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
Intel Core i5-14500, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.406
  [Host]     : .NET 8.0.7 (8.0.724.31311), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.13 (8.0.1325.6609), X64 RyuJIT AVX2


```
| Method                | Mean      | Error      | StdDev     |
|---------------------- |----------:|-----------:|-----------:|
| RunAsyncARE           |  72.97 ms |   1.439 ms |   2.282 ms |
| RunAsyncSemaphoreSlim |  73.32 ms |   1.424 ms |   2.174 ms |
| RunAsyncSpinWait      | 914.86 ms | 287.024 ms | 804.847 ms |
