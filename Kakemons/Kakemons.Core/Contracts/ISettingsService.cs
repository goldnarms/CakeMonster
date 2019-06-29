using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using DynamicData;
using Energihjem.Common.Enums;
using Energihjem.Common.Models;
using Energihjem.Common.Models.Dtos;
using Energihjem.Common.Models.Responses;

namespace Energihjem.Mobile.Core.Contracts
{
    public interface ISettingsService
    {
        string CustomerDashboardUrl { get; set; }
        string ContactCustomerDashboardUrlTitle { get; }
        string ContactCustomerDashboardUrl { get; }
        string ContactBecomeCustomerUrlTitle { get; }
        string ContactBecomeCustomerUrl { get; }
        string ContactFacebookUrlTitle { get; }
        string ContactFacebookUrl { get; }
        string ContactSupportEmailAddressTitle { get; }
        string ContactSupportEmailAddress { get; }
        string ContactUserAgreementTitle { get; }
        string ContactUserAgreementUrl { get; }
        string ContactStoreUrlTitle { get; }
        string ContactStoreUrl { get; }
        string ContactPhoneNumberTitle { get; }
        string ContactPhoneNumber { get; }
        string ContactEmailAddressTitle { get; }
        string ContactEmailAddress { get; }
        string ContactCustomerNewUser { get; }
        SpotPricePlace DefaultSpotPricePlace { get; set; }
        string HeatControlSystemExternalUrl { get; }
        string HeatControlSystemExternalUrlTitle { get; }
        string HeatControlSystemMeasureName { get; }
        string VentilationExternalUrl { get; }
        string VentilationExternalUrlTitle { get; }
        string VentilationMeasureName { get; }
        string WindowExternalUrl { get; }
        string WindowExternalUrlTitle { get; }
        string WindowMeasureName { get; }
        string WallExternalUrl { get; }
        string WallExternalUrlTitle { get; }
        string WallMeasureName { get; }
        string HeatpumpExternalUrl { get; }
        string HeatpumpExternalUrlTitle { get; }
        string HeatpumpMeasureName { get; }
        bool ShowSpeedometer { get; }
        bool ShowAdvisor { get; }
        bool ShowOrderElectrician { get; }
        bool ShowEnergyPortal { get; }
        bool ShowChatPower { get; }
        bool ShowChatBroadband { get; }
        bool ShowPilotCustomerSignup { get; }
        string PilotUrl { get; }
        string OrderElectricianUrl { get; }
        string AdvisorUrl { get; }
        string EnergyPortalUrl { get; }
        string ChatOrganizationId { get; }
        string ChatUrl { get; }
        bool ShowChat { get; }
        bool ShowSolarPower { get; }
        string SolarPowerUrl { get; }
        string BroadbandButtonId { get; }
        string PowerButtonId { get; }
        string BroadbandDeploymentId { get; }
        string PowerDeploymentId { get; }
        int ProviderId { get; }
        int SalesforceApiVersion { get; }
        string AccessToken { get; }
        bool IsNewUser { get; set; }
        Vendor AppVendor { get; }
        string AppCenterId { get; }
        string BaseApiUrl { get; }
        string ContactAddressStreet { get; }
        string ContactAddressCity { get; }
        string ContactAddressPostalCode { get; }
        string SolarPowerTitle { get; }
        IObservableList<ExternalUrlDto> SocialMediaItems { get; }
        string ExternalLinkAppGirMeriOSTitle { get; }
        bool ExternalLinkAppGirMeriOSActive { get; }
        string ExternalLinkAppGirMeriOSFallbackUrl { get; }
        string ExternalLinkAppGirMeriOSUrl { get; }
        string ExternalLinkAppGirMerAndroidTitle { get; }
        bool ExternalLinkAppGirMerAndroidActive { get; }
        string ExternalLinkAppGirMerAndroidFallbackUrl { get; }
        string ExternalLinkAppGirMerAndroidUrl { get; }
        bool ExternalLinkWebUrlActive { get; }
        string ExternalLinkWebUrlTitle { get; }
        string ExternalLinkWebUrl { get; }
        bool ExternalLinkSnapchatUrlActive { get; }
        string ExternalLinkSnapchatUrlTitle { get; }
        string ExternalLinkSnapchatUrl { get; }
        bool ExternalLinkInstagramUrlActive { get; }
        string ExternalLinkInstagramUrlTitle { get; }
        string ExternalLinkInstagramUrl { get; }
        bool ExternalLinkLinkedInUrlActive { get; }
        string ExternalLinkLinkedInUrlTitle { get; }
        string ExternalLinkLinkedInUrl { get; }
        bool ExternalLinkTwitterUrlActive { get; }
        string ExternalLinkTwitterUrlTitle { get; }
        string ExternalLinkTwitterUrl { get; }
        bool ExternalLinkFacebookUrlActive { get; }
        string ExternalLinkFacebookUrlTitle { get; }
        string ExternalLinkFacebookUrl { get; }
        string ExternalLinkAppSpotPriceiOSTitle { get; }
        bool ExternalLinkAppSpotPriceiOSActive { get; }
        string ExternalLinkAppSpotPriceiOSFallbackUrl { get; }
        string ExternalLinkAppSpotPriceiOSUrl { get; }
        string ExternalLinkAppSpotPriceAndroidTitle { get; }
        bool ExternalLinkAppSpotPriceAndroidActive { get; }
        string ExternalLinkAppSpotPriceAndroidFallbackUrl { get; }
        string ExternalLinkAppSpotPriceAndroidUrl { get; }
        AppEnvironment AppEnvironment { get; }
        bool NewsLetterActive { get; }
        string NewsLetterTitle { get; }
        string NewsLetterUrl { get; }
        IObservable<Unit> UpdateFromAppSettings(DynamicAppSettings settings, long created);
        void UpdateFromProviderSettings(ProviderDto providerSettings, long created);
        Task<AuthTokens> GetToken();
        Task SetNewToken();
        Task Logout();
        Task Login(AccessTokenResponse accessToken);
    }
}