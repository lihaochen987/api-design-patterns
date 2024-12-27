// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.

namespace backend.Supplier.DomainModels;

public class PhoneNumber
{
    public long SupplierId { get; set; }
    public string CountryCode { get; set; }
    public long AreaCode { get; set; }
    public long Number { get; set; }
}
