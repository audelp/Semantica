#include <stdio.h>
#include <math.h>

int altura,i,j;
float x,y,c;

void main()
{
    y = 10;
    x = 9;
    x ++;
    x --;
    x = (3 + 5) * 8 - (10 - 4) / 2; 
    x /= (y-3); 
    x --; 
    x *= (x-7);
    v -= 3+5;
    
    printf("\nValor de altura = ");
    scanf("%d",&altura);

    for (i = 1; i<=altura; i++)
    {
        for (j = 1; j<=i; j++)
        {
            printf("",j);
        }
        printf("\n");
    }
    i = 0;
    do
    {
        printf("-");
        i++;
    }
    while (i<altura*2);
    printf("\n");
    for (i = 1; i<=altura; i++)
    {
        j = 1;
        while (j<=i)
        { 
            printf("",j);
            j++;
        }
        printf("\n");
    }
    i = 0;
    do
    {
        printf("-");
        i++;
    }
    while (i<altura*2);
    printf("\n");
}