<Query Kind="Statements">
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.0\libaoc.dll">C:\proj\advent-of-code\libaoc\bin\Debug\netstandard2.0\libaoc.dll</Reference>
  <Namespace>AoC</Namespace>
</Query>

var rc = 0;

for (var i = 1; i <= 10551348; ++i)
    if ((10551348 % i) == 0)
        rc += i;

rc.Dump();
