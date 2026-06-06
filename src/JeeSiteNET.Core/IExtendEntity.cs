namespace JeeSiteNET.Core;

public interface IExtendEntity
{
    string? ExtendS1 { get; set; }
    string? ExtendS2 { get; set; }
    string? ExtendS3 { get; set; }
    string? ExtendS4 { get; set; }
    string? ExtendS5 { get; set; }
    string? ExtendS6 { get; set; }
    string? ExtendS7 { get; set; }
    string? ExtendS8 { get; set; }

    int? ExtendI1 { get; set; }
    int? ExtendI2 { get; set; }
    int? ExtendI3 { get; set; }
    int? ExtendI4 { get; set; }

    decimal? ExtendF1 { get; set; }
    decimal? ExtendF2 { get; set; }
    decimal? ExtendF3 { get; set; }
    decimal? ExtendF4 { get; set; }

    DateTime? ExtendD1 { get; set; }
    DateTime? ExtendD2 { get; set; }
    DateTime? ExtendD3 { get; set; }
    DateTime? ExtendD4 { get; set; }

    string? ExtendJson { get; set; }
}
