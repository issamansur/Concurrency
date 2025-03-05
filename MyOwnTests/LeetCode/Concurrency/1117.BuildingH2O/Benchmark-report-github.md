```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method                          | Mean     | Error   | StdDev   |
|-------------------------------- |---------:|--------:|---------:|
| TaskRunWithBoolAndARE           | 454.6 μs | 8.97 μs | 20.06 μs |
| TaskRunWithBarrierAndSemaphores | 156.5 μs | 2.47 μs |  2.19 μs |
