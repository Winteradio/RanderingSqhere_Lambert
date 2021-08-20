using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace 원_드로잉
{
    public partial class Form1 : Form
    {
        // Declare Position Variable in Int >> Because I thought Int Position information can be used easy then double or float Position information
        public double z;

        // Declare Degree of Bright in Int >> Because Bright in Computer monitor is cognized to degree of pixel is one to one
        public int nBright;

        // Declare Position Variable of Sun in Int
        public double lx = 1;
        public double ly = 1;
        public double lz = 1;

        // Declare Radius
        public double radius=200;

        // Declare Light of Direct and Ambient
        public int Direct = 255;
        public int Ambient = 50;

        public double L;
        public double Dot;

        public Form1()
        {
            InitializeComponent();
        }

        // Z축의 좌표 결정
        // 정면에서 보이는 쪽이 +라고 생각하면 튀어나오는게 + : 정면에서 보이는 쪽이 -라고 생각하면 들어가는게 +

        // 3D 구면에서의 밝기
        // 법선 벡터가 정확히 광원 쪽을 향하면 밝기는 최대
        // 법선 벡터가 광원에서 PI/2(90도)이면 발기는 최소가 된다 (또 면이 광원과 역방향이면 그 면에는 광원 빛이 직접 닿지 않으므로 다음과 같이 말할 수 있다)
        // 법선 벡터가 광원에 PI/2(90도) 이상이면 밝기가 최소가 된다
        // 0 ~ 90도까지는 밝기가 확실히 서서히 어두워진다
        // 90도 ~ 180도까지는 그렇다면 어떻게 감소하는 것일까?
        // 빛의 감소 및 증가에 관해서는 "람베르트 반사"를 활용하자

        // "람베르트 반사" : 밝기의 변화는 물체에 따라 다르다 >> 물체의 질감에 따라 밝기의 변화정도가 바뀐다
        // I = I0 + I1*cosE
        // I : 색의 밝기
        // I0 : 환경광의 세기 >> 환경광은 광원의 각도와 관계없이 일정한 밝기로 면을 비추는 빛, 모든 방향에서 일정한 밝기로 오는 빛을 의미한다.
        // I1 : 직사광의 세기
        // E : 광원과 면의 법선벡터가 이루는 각

        // 벡터의 내적 : A=(ax,ay,az) , B=(bx,by,bz)
        // A*B = ax*bx + ay*by + az+bz
        // L : 광원 방향의 단위벡터 >> 단위벡터는 크기가 1인 벡터를 의미한다.
        // N : 면의 단위 법선 벡터 >> N = dx/Rad + dy/Rad + dz/Rad
        // L*N = lx*nx + ly*ny + lz*nz
        // I = I0 + I1*cosE = I0 + I1 * ( lx*nx + ly*ny + lz*nz)
        
        // 벡터의 내적값의 범위 0 ~ 1
        // 0 : 90도 , 1 : 0도
        // 벡터의 내적값 <0 일 경우 : 빛과 지정된 면이 닿는다는 의미로 해석 가능 >> "람베르트 반사" 공식 활용
        // 벡터의 내적값 >=0 일 경우 : 빛과 지정된 면이 닿지않는다는 의미로 해석 가능 >> 90 ~ 180도의 빛의 밝기는 모두 동일하다고 생각
      

        private void button1_Click(object sender, EventArgs e)
        {
            // PictureBox Coodrination System
            // (Left Up Position) = (0,0) , (Right Down Position) = (Max,Max)
            Graphics g = pictureBox1.CreateGraphics();

            // L is size of Light Vector
            L = Math.Sqrt(Math.Pow(lx, 2) + Math.Pow(ly, 2) + Math.Pow(lz, 2));

            // I draw Picturebox going >>> and down line, repeating until draw at all 
            for (int j =0; j < pictureBox1.Height; j++)
            {
                for (int i = 0; i < pictureBox1.Width; i++)
                {

                    // This mean is "Only draw in Sqhere Area
                    if (Math.Pow(radius,2) - (Math.Pow(i-pictureBox1.Width/2,2)+Math.Pow(j-pictureBox1.Height/2,2)) > 0)
                    {
                        // z is Position Vector Z of Sqhere 
                        z = Math.Sqrt(Math.Pow(radius, 2) - (Math.Pow(i - pictureBox1.Width / 2, 2) + Math.Pow(j - pictureBox1.Height / 2, 2)));
                        // I use Convet.ToDouble, because 'int'/'int' or 'int'/'double''s result 'int', too.
                        // But I must need to use 'double' result in 'Dot'
                        Dot = (Convert.ToDouble(i-pictureBox1.Width/2)/radius)*(lx/L)+ (Convert.ToDouble(j - pictureBox1.Height / 2) / radius) * (ly / L) + (z/radius)*(ly/L);
                        
                        // "Dot <0"'s mean is angle of Light Vector between Sqhere Linear Position Vector is 180 ~ 90
                        // "Dot >=0" 's mean is angle of Light Vector between Sqhere Linear Position Vector is 0 ~ 90 >> Degree of Light have been decreasing
                        if (Dot < 0)
                        {
                            if ((-Dot) * Direct+Ambient > 255)
                            {
                                nBright = 255;
                            }
                            else
                            {
                                nBright = Convert.ToInt32((-Dot) * Direct+Ambient);
                            }
                        }
                        else
                        {
                            nBright = Convert.ToInt32(Ambient);
                        }
                        Pen mypen = new Pen(Color.FromArgb(nBright, nBright, nBright, nBright), 0.1f);
                        g.DrawLine(mypen, i, j, i+1, j);
                    }
                }
            }
        }
    }
}
