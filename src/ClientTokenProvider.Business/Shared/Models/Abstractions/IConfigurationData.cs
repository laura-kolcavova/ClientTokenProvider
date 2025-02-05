﻿using ClientTokenProvider.Core.AzureAd.Models;

namespace ClientTokenProvider.Business.Shared.Models.Abstractions;

public interface IConfigurationData
{
    public string Scope { get; }

    public IClientTokenProviderConfiguration ToClientTokenProviderConfiguration();
}
