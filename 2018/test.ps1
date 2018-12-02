dir -recur *.linq | %{ write-host -nonew "Testing $($_.name)..."; lprun $_ }
