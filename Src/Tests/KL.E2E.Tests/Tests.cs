using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;

namespace KL.E2E.Tests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task Login_As_InternalUser_See_ResourceActions_And_Logout()
    {
        await Page.GotoAsync("https://keeplearning-as.azurewebsites.net/");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).First.ClickAsync();
        await Page.WaitForResponseAsync(resp =>
            resp.Url.Contains("/api/Resources") && resp.Status == 200);

        await Page.ScreenshotAsync(new()
        {
            Path = "screenshot.internal.png",
        });

        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Nieuw" })).ToBeVisibleAsync();

        await Expect(Page.Locator(".mud-card-header-actions > .mud-button-root").First)
            .ToBeVisibleAsync();

        await Expect(Page.Locator("h4")).ToContainTextAsync("Resources");


        await Page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).First)
            .ToBeVisibleAsync();
    }

    [Test]
    public async Task Login_As_CommunityUser_ResourceActions_Should_Not_Be_Visible_And_Logout()
    {
        await Page.GotoAsync("https://keeplearning-as.azurewebsites.net/");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).Nth(1).ClickAsync();

        await Page.WaitForResponseAsync(resp =>
            resp.Url.Contains("api/Resources") && resp.Status == 200);

        await Page.ScreenshotAsync(new()
        {
            Path = "screenshot.community.png",
        });

        await Expect(Page.Locator(".mud-card-header-actions").First)
            .ToBeEmptyAsync();

        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Nieuw" })).ToBeHiddenAsync();

        await Expect(Page.Locator("h4")).ToContainTextAsync("Resources");

        await Page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();

        await Expect(Page.GetByRole(AriaRole.Button, new() { Name = "Login" }).First)
            .ToBeVisibleAsync();
    }
}