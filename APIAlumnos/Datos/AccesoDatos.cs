namespace APIAlumnos.Datos
{
    public class AccesoDatos // esta clase se usa para gestionar la cadena de conexion por si tenemos mas de una. tiene un variable privada que tiene una propiedad publica a traves de la cual se puede obtener su valor. y el constructor es el que asigna el valor que se le pasa, y se lo asigna a dicha cadena.
    {
        private string cadenaConexionSql;
        public string CadenaConexionSQL { get => cadenaConexionSql; }  // propiedad publica a traves de la cual se puede acceder al valor de la variable privada cadenaConexionSql
        public AccesoDatos(string ConexionSql)  // si mas adelante necesito usar otra cadena puedo sobrecargar el constructor o añadir otro parametro con la otra cadena
        {
            cadenaConexionSql = ConexionSql;
        }

    }
}


