﻿namespace Chat.Application.Common;

public class FilterContext
{
    public Guid? UserId { get; set; } = Guid.Empty;
    public OrderInfo? OrderInfo { get; set; } = new OrderInfo();
    public SearchInfo? SearchInfo { get; set; } = new SearchInfo();
}

public class OrderInfo
{
    public OrderField OrderField { get; set; } = OrderField.Date;
    public bool Ascending { get; set; } = false;
}

public class SearchInfo
{
    public SearchField SearchField { get; set; } = SearchField.Chats;
    public string? SearchText { get; set; } = string.Empty;
    public DateTime? DateCreateChat { get; set; }
}

public enum OrderField
{
    Date,
    Title,
}

public enum SearchField
{
    Users,
    Messages,
    Chats,
}
