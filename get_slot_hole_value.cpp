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
        int web, flg;
        cout << "height(web) : ";
        cin >> web;
        cout << "width(flg) : ";
        cin >> flg;

        int A = web;
        int A1 = web + 30;
        int B = flg + 30;
        int B1 = flg + 45;
        int C, R;
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

        int R1;
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

        int D;
        if (web <= 250)
        {
            D = 50;
        }
        else
        {
            D = 70;
        }

        int R2, F;
        if (web <= 125)
        {
            R2 = F = 30;
        }
        else
        {
            R2 = F = 40;
        }

        int D1;
        if (web <= 125)
        {
            D1 = 30;
        }
        else
        {
            D1 = 50;
        }

        int E;
        if (web <= 125)
        {

            E = 30;
        }
        else
        {

            E = 35;
        }

        printf("AH => B : %d, A1 : %d, R2 : %d\n", B, A1, R2);
        printf("AA => B : %d, A1 : %d, R2 : %d, R : %d, E : %d, D1 : %d\n", B, A1, R2, R, E, D1);
        printf("AG => B : %d, A1 : %d, R : 50고정\n", B, A1);
        printf("AJ => A1 : %d, R1 : %d, R : 50고정\n", A1, R1);
    }

    if (type == "T")
    {
        int web, face;
        cout << "height(web) => A 의 높이 - Thick_F: ";
        cin >> web;
        cout << "width(face) : ";
        cin >> face;

        int A = web;
        int B = face;
        int F = face + 30;
        int L = face + 60;

        int C, R;
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

        printf("TE => B : %d, L : %d, R : 50고정\n", B, L);
        printf("TG => B : %d, R : %d\n", B, R);
    }
}
