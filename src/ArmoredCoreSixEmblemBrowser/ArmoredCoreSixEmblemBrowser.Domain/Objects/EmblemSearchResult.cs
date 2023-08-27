using ArmoredCoreSixEmblemBrowser.Entities;

namespace ArmoredCoreSixEmblemBrowser.Domain.Objects;

public record EmblemSearchResult(
    int TotalEmblems, List<Emblem> Emblems);