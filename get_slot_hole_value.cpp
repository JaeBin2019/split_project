#include <iostream>
#include <algorithm>

using namespace std;

int main()
{

    string type;
    cout << "Type A or T : ";
    cin >> type;

    if (type == "A")
    {
        float web, flg, thick_w;
        cout << "height(web) : ";
        cin >> web;
        cout << "width(flg) : ";
        cin >> flg;
        cout << "thick_w : ";
        cin >> thick_w;

        float A = web;
        float A1 = web + 30;
        float B = flg + 30;
        float B1 = flg + 45;
        float C, R;
        if (web <= 150)
        {
            C = R = 0;
        }
        else if (web <= 300)
        {
            C = R = 50;
        }
        else if (web <= 450)
        {
            C = R = 75;
        }
        else
        {
            C = R = 100;
        }

        float R1;
        if (web <= 200)
        {
            R1 = 0;
        }
        else if (web <= 300)
        {
            R1 = 50;
        }
        else if (web <= 450)
        {
            R1 = 75;
        }
        else
        {
            R1 = 100;
        }

        float D;
        if (web <= 250)
        {
            D = 50;
        }
        else
        {
            D = 70;
        }

        float R2, F;
        if (web <= 125)
        {
            R2 = F = 30;
        }
        else
        {
            R2 = F = 40;
        }

        float D1;
        if (web <= 125)
        {
            D1 = 30;
        }
        else
        {
            D1 = 50;
        }

        float E;
        if (web <= 125)
        {

            E = 30;
        }
        else
        {

            E = 35;
        }

        printf("AH => B : %.1f, A1 : %.1f, R2 : %.1f\n", B, A1, R2);
        printf("AA => B : %.1f, A1 : %.1f, R2 : %.1f, R : %.1f, E : %.1f, D1 : %.1f\n", B, A1, R2, R, E, D1);
        printf("AG => thick_w : %.1f, B - thick_w: %.1f, A1 : %.1f, flg - thickW + 40: %.1f, R : 50고정\n", thick_w, B - thick_w, A1, flg - thick_w);
        printf("AJ => A1 : %.1f, R1 : %.1f, flg + 40 : %.1f, R : 50고정\n", A1, R1, flg + 40);
        printf("plate => 높이 : %.1f, 폭 : %.1f, R : %.1f\n", web - D, flg - thick_w + C + A, R);
    }

    if (type == "T")
    {
        float web, face, thick_w;
        cout << "height(web) => A 의 높이 : ";
        cin >> web;
        cout << "width(face) : ";
        cin >> face;
        cout << "thick_w : ";
        cin >> thick_w;

        float A = web;
        float B = face;
        float F = face + 30;
        float L = face + 60;

        float C, R;
        if (web <= 250)
        {
            C = R = 0;
        }
        else if (web <= 350)
        {
            C = R = 50;
        }
        else if (web <= 450)
        {
            C = R = 75;
        }
        else
        {
            C = R = 100;
        }
        float left = (B - thick_w) / 2;
        float right = (B - thick_w) / 2 + thick_w;

        printf("TE => left : %.2f, right : %.2f, height : %.1f\n", left + 50, right + 50, web + 50);
        printf("TG => B : %.1f, R : %.1f\n", B, R);
    }
}
