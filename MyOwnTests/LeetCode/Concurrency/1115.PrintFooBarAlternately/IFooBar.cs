namespace MyOwnTests.LeetCode.Concurrency._1115.PrintFooBarAlternately;

public interface IFooBar
{
    void Foo(Action printFoo);
    void Bar(Action printBar);
}