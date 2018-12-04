dir -recur day*.linq | %{ write-host -nonew "Testing $($_.name)..."; lprun $_ }
