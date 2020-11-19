using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SharedSamplerTest
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Effect effect;
        VertexPositionTextureType[] vertices;
        int[] indices;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                GraphicsProfile = GraphicsProfile.HiDef
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = Content.Load<Effect>("TestEffect");
            effect.Parameters["xView"].SetValue(Matrix.CreateLookAt(new Vector3(0, 0, 10), Vector3.Zero, Vector3.Up));
            effect.Parameters["xProjection"].SetValue(Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GraphicsDevice.Viewport.AspectRatio, 1.0f, 100.0f));
            effect.Parameters["TextureSampler+xTexture0"].SetValue(Content.Load<Texture2D>("Red"));
            effect.Parameters["TextureSampler+xTexture1"].SetValue(Content.Load<Texture2D>("Green"));

            vertices = new VertexPositionTextureType[8];
            indices = new int[12];

            for (int i = 0; i < 2; i++)
            {
                Vector3 positionOffset = Vector3.Zero;
                if (i == 0)
                    positionOffset.X = -2f;
                if (i == 1)
                    positionOffset.X = 2f;

                int vertexOffset = (4 * i);
                vertices[vertexOffset + 0].Position = positionOffset + new Vector3(-1, -1, 0);
                vertices[vertexOffset + 0].TextureCoordinate = new Vector2(-1, -1);
                vertices[vertexOffset + 0].TextureType = i % 2;
                vertices[vertexOffset + 1].Position = positionOffset + new Vector3(1, -1, 0);
                vertices[vertexOffset + 1].TextureCoordinate = new Vector2(1, -1);
                vertices[vertexOffset + 1].TextureType = i % 2;
                vertices[vertexOffset + 2].Position = positionOffset + new Vector3(-1, 1, 0);
                vertices[vertexOffset + 2].TextureCoordinate = new Vector2(-1, 1);
                vertices[vertexOffset + 2].TextureType = i % 2;
                vertices[vertexOffset + 3].Position = positionOffset + new Vector3(1, 1, 0);
                vertices[vertexOffset + 3].TextureCoordinate = new Vector2(1, 1);
                vertices[vertexOffset + 3].TextureType = i % 2;

                int indiceOffset = (6 * i);
                indices[indiceOffset + 0] = vertexOffset + 0;
                indices[indiceOffset + 1] = vertexOffset + 3;
                indices[indiceOffset + 2] = vertexOffset + 1;
                indices[indiceOffset + 3] = vertexOffset + 0;
                indices[indiceOffset + 4] = vertexOffset + 2;
                indices[indiceOffset + 5] = vertexOffset + 3;

                vertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionTextureType.VertexDeclaration, vertices.Length, BufferUsage.WriteOnly);
                vertexBuffer.SetData(vertices);
                indexBuffer = new IndexBuffer(GraphicsDevice, typeof(int), indices.Length, BufferUsage.WriteOnly);
                indexBuffer.SetData(indices);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.Indices = indexBuffer;
                GraphicsDevice.SetVertexBuffer(vertexBuffer);
                GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, indices.Length / 3);
            }

            base.Draw(gameTime);
        }
    }

    public struct VertexPositionTextureType
    {
        public Vector3 Position;
        public Vector2 TextureCoordinate;
        public float TextureType;

        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float) * 3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof(float) * 5, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
        );
    }
}
