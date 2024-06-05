using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MpDemo.Client;
using MpDemo.Graphics;

using Steamworks;

namespace MpDemo;

public class Main : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private bool steamFailed = false;
    private Texture2D? pfp;
    private string username;

    private SpriteFont font = null;

    public Main()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        try
        {
            if(SteamAPI.RestartAppIfNecessary((AppId_t)SteamManager.steam_appid))
            {
                Console.Out.WriteLine("Game wasn't started by Steam-client. Restarting.");
                Exit();
            }
        }
        catch(DllNotFoundException e)
        {
            // We check this here as it will be the first instance of it.
            Console.WriteLine("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e);
            steamFailed = true;
        }
    }

    protected override void Initialize()
    {
        MainWindow.SetBounds(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        if(!steamFailed && SteamManager.Init())
            Exiting += Game_Exiting;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // font = new BinaryFontImporter(Content).Import("Default");
        font = Content.Load<SpriteFont>("Fonts/Default");

        if(SteamManager.IsSteamRunning)
        {
            pfp = GetSteamUserAvatar(GraphicsDevice);
            username = SteamFriends.GetPersonaName();
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if(SteamManager.IsSteamRunning)
            SteamAPI.RunCallbacks();

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        if(SteamManager.IsSteamRunning)
        {
            if(pfp != null)
            {
                _spriteBatch.Draw(pfp, new Vector2(20, 20), Color.White);
                _spriteBatch.DrawString(font, username, new Vector2(20, 40) + Vector2.UnitY * pfp.Height, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            }
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    private void Game_Exiting(object sender, EventArgs e)
    {
        if(SteamManager.IsSteamRunning)
            SteamAPI.Shutdown();
    }

    private static Texture2D GetSteamUserAvatar(GraphicsDevice device)
    {
        // Get the icon type as a integer.
        var icon = SteamFriends.GetMediumFriendAvatar(SteamUser.GetSteamID());

        // Check if we got an icon type.
        if (icon != 0)
        {
            var ret = SteamUtils.GetImageSize(icon, out uint width, out uint height);

            if (ret && width > 0 && height > 0)
            {
                var rgba = new byte[width * height * 4];
                ret = SteamUtils.GetImageRGBA(icon, rgba, rgba.Length);
                if (ret)
                {
                    var texture = new Texture2D(device, (int)width, (int)height, false, SurfaceFormat.Color);
                    texture.SetData(rgba, 0, rgba.Length);
                    return texture;
                }
            }
        }
        return null;
    }
}
