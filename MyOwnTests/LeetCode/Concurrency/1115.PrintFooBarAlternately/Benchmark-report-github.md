```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4890/23H2/2023Update/SunValley3)
Unknown processor
.NET SDK 8.0.101
  [Host]     : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.1 (8.0.123.58001), X64 RyuJIT AVX2


```
| Method               | Mean       | Error     | StdDev      |
|--------------------- |-----------:|----------:|------------:|
| RunWithARE           |   114.8 ms |   2.26 ms |     3.90 ms |
| RunWithARE2          |   113.7 ms |   2.20 ms |     3.30 ms |
| RunWithSemaphoreSlim |   113.4 ms |   1.92 ms |     2.14 ms |
| RunWithSpinWait      | 4,405.5 ms | 492.76 ms | 1,445.19 ms |
