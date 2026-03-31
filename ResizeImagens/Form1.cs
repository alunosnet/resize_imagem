using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ResizeImagens
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog f=new OpenFileDialog();

            if (f.ShowDialog() == DialogResult.OK)
            {
                string ficheiro = f.FileName;
                Stream fstream = new FileStream(ficheiro, FileMode.Open, FileAccess.Read);
                // Carrega a imagem do upload
                using (Image originalImage = Image.FromStream(fstream))
                {
                    //int novaLargura = 800;
                    //int novaAltura = 600;
                    //Image img = RedimensionarImagem(originalImage, novaLargura, novaAltura);
                    //reduzir em 50%
                    Image img = RedimensionarImagem(originalImage, 50);
                    img.Save("novaimagem");
                    pictureBox1.Image = img;
                }
            }
        }
        private Image RedimensionarImagem(Image imagem, int largura, int altura)
        {
            var destRect = new Rectangle(0, 0, largura, altura);
            var destImage = new Bitmap(largura, altura);

            // Mantém DPI original
            destImage.SetResolution(imagem.HorizontalResolution, imagem.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                // Qualidade alta
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(imagem, destRect, 0, 0, imagem.Width, imagem.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        private Image RedimensionarImagem(Image imagem,int percentagem)
        {
            //Calcula a nova resolução com base na percentagem
            Size novo = CalcularTamanhoproporcional(imagem.Width, imagem.Height, percentagem);
            //redimensiona a imagem
            return RedimensionarImagem(imagem, (int)novo.Width, (int)novo.Height);
        }
        /// <summary>
        /// Devolve a nova resolução mantendo a relação largura/altura
        /// </summary>
        /// <param name="larguraOriginal"></param>
        /// <param name="alturaOriginal"></param>
        /// <param name="larguraMax"></param>
        /// <param name="alturaMax"></param>
        /// <returns></returns>
        private Size CalcularTamanhoproporcional(int larguraOriginal, int alturaOriginal, int larguraMax, int alturaMax)
        {
            double ratioX = (double)larguraMax / larguraOriginal;
            double ratioY = (double)alturaMax / alturaOriginal;
            double ratio = Math.Min(ratioX, ratioY);

            return new Size(
                (int)(larguraOriginal * ratio),
                (int)(alturaOriginal * ratio)
            );
        }
        /// <summary>
        /// Devolve o tamanho da imagem com base na percentagem indicada
        /// </summary>
        /// <param name="larguraOriginal"></param>
        /// <param name="alturaOriginal"></param>
        /// <param name="percentagem"></param>
        /// <returns></returns>
        private Size CalcularTamanhoproporcional(float larguraOriginal, float alturaOriginal, int percentagem)
        {
            

            return new Size(
                (int)(larguraOriginal * ((float)percentagem/100)),
                (int)(alturaOriginal * ((float)percentagem/100))
            );
        }


    }
}
