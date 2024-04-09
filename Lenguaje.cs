using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*Requerimento 1: Marcar errores sintacticos para variables no declaradas
Requerimiento 1: Marcar errores sintacticos para variables no declaradas
Requerimiento 2: Asignacion, modificar el valor de la variable, no pasar por alto el ++ y el --
Requerimiento 3: Printf: Implementar secuencias de escape, quitar comillas
Requerimiento 4: Modificar el valor de la variable en el Scanf y levantar una excepción si lo capturado no es un numero
Requerimiento 5: Implementar el casteo
requerimiento 6: /n  /t*/

namespace Semantica
{
    public class Lenguaje : Sintaxis
    {
        List<Variable> variables;
        public Lenguaje()
        {
            variables = new List<Variable>();
        }
        public Lenguaje(string nombre) : base(nombre)
        {
            variables = new List<Variable>();
        }
        //Programa  -> Librerias? Variables? Main
        public void Programa()
        {
            if (getContenido() == "#")
            {
                Librerias();
            }
            if (getClasificacion() == Tipos.tipoDatos)
            {
                Variables();
            }
            Main();
            imprimeVariables();
        }
        private void imprimeVariables()
        {
            log.WriteLine("Variables: ");
            foreach (Variable v in variables)
            {
                log.WriteLine(v.getNombre() +" = "+v.getTipo() +" = " + v.getValor());
            }
        }
        private bool existeVariable(string nombre)
        {
            foreach (Variable v in variables)
            {
                
                if(nombre == v.getNombre())
                {
                    return true;
                }
            }
            return false;
        }
        private void variableDeclarada(String nombre)
        {
            foreach (Variable v in variables)
            {
                if(){
                    
                }
                if(nombre == v.getNombre())
                {
                }
                else
                    throw new Error("de Sintaxis : la variable " + nombre + " no esta declarada",log,linea);
            }


        }
        //Librerias -> #include<identificador(.h)?> Librerias?
        private void Librerias()
        {
            match("#");
            match("include");
            match("<");
            match(Tipos.Identificador);
            if (getContenido() == ".")
            {
                match(".");
                match("h");
            }
            match(">");
            if (getContenido() == "#")
            {
                Librerias();
            }
        }
        //Variables -> tipoDato listaIdentificadores; Variables?
        private void Variables()
        {
            Variable.TipoDato tipo = Variable.TipoDato.Char;
            switch(getContenido())
            {
                case "int":
                    tipo = Variable.TipoDato.Int;
                    break;
                case "float":
                    tipo = Variable.TipoDato.Float;
                    break;
            }
            match(Tipos.tipoDatos);
            listaIdentificadores(tipo);
            match(";");
            if (getClasificacion() == Tipos.tipoDatos)
            {
                Variables();
            }
        }
        //listaIdentificadores -> Identificador (,listaIdentificadores)?
        private void listaIdentificadores(Variable.TipoDato tipo)
        {
            string nombre = getContenido();
            match(Tipos.Identificador);
            if(!existeVariable(nombre))
            {
                variables.Add(new Variable(nombre,tipo));

            }
            else
            {
                throw new Error("de Sintaxis : la variable " + nombre + " ya existe",log,linea);
            }
            if (getContenido() == ",")
            {
                match(",");
                listaIdentificadores(tipo);
            }
        }
        //bloqueInstrucciones -> { listaIntrucciones? }
        private void bloqueInstrucciones()
        {
            match("{");
            if (getContenido() != "}")
            {
                ListaInstrucciones();

            }
            match("}");
        }
        //ListaInstrucciones -> Instruccion ListaInstrucciones?
        private void ListaInstrucciones()
        {
            Instruccion();
            if (getContenido() != "}")
            {
                ListaInstrucciones();
            }
        }
        //Instruccion -> Printf | Scanf | If | While | do while | For | Asignacion
        private void Instruccion()
        {
            if (getContenido() == "printf")
            {
                Printf();
            }
            else if (getContenido() == "scanf")
            {
                Scanf();
            }
            else if (getContenido() == "if")
            {
                If();
            }
            else if (getContenido() == "while")
            {
                While();
            }
            else if (getContenido() == "do")
            {
                Do();
            }
            else if (getContenido() == "for")
            {
                For();
            }
            else
            {
                Asignacion();
            }
        }
        //    Requerimiento 1: Printf -> printf(cadena(, Identificador)?);
        private void Printf()
        {
            match("printf");
            match("(");
            match(Tipos.Cadena);
            while (getContenido() == ",")
            {
                match(",");
                match(Tipos.Identificador);//1
            }
            match(")");
            match(";");

        }
        //    Requerimiento 2: Scanf -> scanf(cadena,&Identificador);
        private void Scanf()
        {
            match("scanf");
            match("(");
            match(Tipos.Cadena);
            match(",");
            match("&");
            match(Tipos.Identificador);//1
            match(")");
            match(";");
        }

        //Asignacion -> Identificador (++ | --) | (+= | -=) Expresion | (= Expresion) ;
        private void Asignacion()
        {
                match(Tipos.Identificador);//1
            if (getClasificacion() == Tipos.IncrementoTermino)
            {
                string operador = getContenido();
                match(Tipos.IncrementoTermino);                
                if (operador == "+=" || operador == "-=")
                {
                    Expresion();
                }
            }
            else if (getClasificacion() == Tipos.IncrementoFactor)
            {
                match(Tipos.IncrementoFactor);
                Expresion();
            }
            else
            {
                match("=");
                Expresion();
            }
            match(";");
        }
        //If -> if (Condicion) instruccion | bloqueInstrucciones 
        //      (else instruccion | bloqueInstrucciones)?
        private void If()
        {
            match("if");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            if(getContenido() == "else")
            {
                match("else");
                if (getContenido() == "{")
                {
                    bloqueInstrucciones();
                }
                else
                {
                    Instruccion();
                }
            }
        }
        //Condicion -> Expresion operadoRelacional Expresion
        private void Condicion()
        {
            Expresion();
            match(Tipos.OperadorRelacional);
            Expresion();
        }
        //While -> while(Condicion) bloqueInstrucciones | Instruccion
        private void While()
        {
            match("while");
            match("(");
            Condicion();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }
        //Do -> do bloqueInstrucciones | Intruccion while(Condicion);
        private void Do()
        {
            match("do");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
            match("while");
            match("(");
            Condicion();
            match(")");
            match(";");
            
        }
        //For -> for(Asignacion Condicion; Incremento) BloqueInstruccones | Instruccion 
        private void For()
        {
            match("for");
            match("(");
            Asignacion();
            Condicion();
            match(";");
            Incremento();
            match(")");
            if (getContenido() == "{")
            {
                bloqueInstrucciones();
            }
            else
            {
                Instruccion();
            }
        }
        //Incremento -> Identificador ++ | --
        private void Incremento()
        {
            match(Tipos.Identificador);//1
            if (getClasificacion() == Tipos.IncrementoTermino)
            {
                match(Tipos.IncrementoTermino);
            }
        }
        //Main      -> void main() bloqueInstrucciones
        private void Main()
        {
            match("void");
            match("main");
            match("(");
            match(")");
            bloqueInstrucciones();
        }
        //Expresion -> Termino MasTermino
        private void Expresion()
        {   
            Termino();
            MasTermino();
        }
        //MasTermino -> (OperadorTermino Termino)?
        private void MasTermino()
        {
            if (getClasificacion() == Tipos.OperadorTermino)
            {
                match(Tipos.OperadorTermino);
                Termino();
            }
        }
        //Termino -> Factor PorFactor
        private void Termino()
        {
            Factor();
            PorFactor();

        }
        //PorFactor -> (OperadorFactor Factor)?
        private void PorFactor()
        {
            if (getClasificacion() == Tipos.OperadorFactor)
            {
                match(Tipos.OperadorFactor);
                Factor();
            }
        }
        //Factor -> numero | identificador | (Expresion)
        private void Factor()
        {
            if (getClasificacion() == Tipos.Numero)
            {
                match(Tipos.Numero);
            }
            else if (getClasificacion() == Tipos.Identificador)
            {
                match(Tipos.Identificador);//1
            }
            else
            {
                match("(");
                Expresion();
                match(")");
            }
        }
    }
}

