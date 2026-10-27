using System;


namespace EntregaFinalFP
{
    internal class Program
    {
        //Van Dentro de la clase pero fuera de los módulos, de esta forma, cada módulo puede verlos y usarlos, y el const significa una constante, osea un valor inalterable en la ejecución del çódigo.
        const int MaxUsuarios = 20; //Máximo número de usuarios
        const int MaxArticulos = 20; //Máximo número de articulos
        const int CC = 0, Nombre = 1, Telefono = 2; //Cada uno de los datos que estarán almacenados en cada columna de la matriz de usuarios
        const int ID = 0, Stock = 2, Precio = 3;//Cada uno de los datos que estarán almacenados en cada columna de la matriz de artículos, menos el nombre que ya está definido
        static string[,] usuarios = new string[MaxUsuarios, 3];//La matriz que almacena los usuarios, con máximo 20 filas (usuarios) y 3 columnas, una para cada dato
        static string[,] articulos = new string[MaxArticulos, 4];//La matriz que almacena los artículos, con máximo 20 filas (artículos) y 4 columnas.
        static int totalArticulos = 0; //Contador para almacenar los articulos totales
        static int totalUsuarios = 0; //Contador para almacenar los usuarios registrados, para saber en qué fila se debe registrar un nuevo usuario
        static void Main(string[] args)
        {
            int opcion;
            do
            {
                MenuPrincipal(); //Muestra el menú principal
                opcion = CapturarOpción(1, 4); //Lee la opción elegida (solo toma valores de 1 a 4
                ElecciónProcesoMenu(opcion); //Ejecuta lo elegido
            } while (opcion != 4); //Se repite hasta que el usuario elija la opción 4 (salir)

            Console.WriteLine("¡Hasta pronto!");//Solo se ejecuta una vez haya terminado el ciclo (O sea cuando el usuario decidió finalizar el programa)
        }

        static void MenuPrincipal() //Solo muestra el menú y las opciones
        {
            Console.Clear();
            Console.WriteLine("----------------------MENÚ-------------------------");
            Console.WriteLine("1.Usuarios");
            Console.WriteLine("2.Artículos");
            Console.WriteLine("3.Venta");
            Console.WriteLine("4.Salir");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Ingrese una opción del menú:");
        }
        static void ElecciónProcesoMenu(int opcion) //Recibe la opción elegida y llama al módulo que corresponde, menos la opción 4 porque esa le corresponde al ciclo
        {
            switch (opcion)
            {
                case 1:
                    MenuUsuarios();
                    break;
                case 2:
                    Articulos();
                    break;
                case 3:
                    Venta();
                    Pausa();//Necesario para mostrar la factura.
                    break;
            }
        }
        static void MenuUsuarios() //Tiene su propio ciclo, para que cuando la opción sea 4 (volver) redirija al usuario al menú principal
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("-----------MENÚ USUARIOS--------------");
                Console.WriteLine("1.Ver Lista de Usuarios");
                Console.WriteLine("2.Nuevo Usuario");
                Console.WriteLine("3.Editar Usuarios");
                Console.WriteLine("4.Volver al Menú Principal");
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Ingrese que operación desea realizar:");

                opcion = CapturarOpción(1, 4);
                switch (opcion)
                {
                    case 1:
                        ListaUsuarios();
                        Pausa();
                        break;
                    case 2:
                        NuevoUsuario();
                        Pausa();
                        break;
                    case 3:
                        EditarUsuario();
                        Pausa();
                        break;
                }
            } while (opcion != 4);

        }
        //UTILIDADES (Módulos que sirven para la mayoria de funciones del programa y que pueden llamar los otros módulos)
        static int CapturarOpción(int min, int max)//Pide un número y no toma hasta quue esté entre el min y el max
        {
            int opcion;
            while (!int.TryParse(Console.ReadLine(), out opcion) || opcion < min || opcion > max)
            {
                Console.WriteLine($"ERROR: Ingrese un número entre {min} y {max}");
            }
            //int.TryParse intenta pasar el texto a número para que el programa no suelte error en caso de que escriban letras, devuelve false si no pudo convertirlo, out opcion es donde deja el número ya convertido, el while se repite mientras que el valor ingresado no sea un número, o esté fuera del rango establecido
            return opcion;//Solo devuelve el valor cuando ya pasó por las validaciones
        }
        //Un proceso similar al anterior pero con texto, el mensaje es lo que se le muestra al usuario, para solicitar cualquier dato de texto
        static string CapturarTexto(string mensaje)
        {
            string texto;
            do
            {
                Console.Write(mensaje);
                texto = Console.ReadLine().Trim();//Trim() quita los espacios vacios, dejando solo los caracteres
                if (texto == "")
                {
                    Console.WriteLine("ERROR: Este campo no puede estar vacío");
                }
            } while (texto == "");
            return texto;
        }
        //Pide un número y no lo suelta hasta que sea mayor que el min (0 para stock y 1 para precio)
        static int CapturarNumero(string mensaje, int min)
        {
            int numero;
            Console.Write(mensaje);
            while (!int.TryParse(Console.ReadLine(), out numero) || numero < min)
            {
                Console.Write($"ERROR: Ingrese un número entero mayor o igual que {min}: ");
            }
            return numero;
        }
        //Busca un usuario por su cédula y devuelve en qué número de fila está almacenado, si no lo encuentra devuelve un valor negativo que no es una fila válida, así quien lo llame puede saber si el usuario solicitado no existe
        static int BuscarUsuario(string cedula)
        {
            for (int i = 0; i < totalUsuarios; i++)
            {
                if (usuarios[i, CC] == cedula)
                {
                    return i;
                }
            }
            return -1;//Solo devuelve un -1 si revisó todas las filas y ninguna coincidió con lo solicitado
        }
        //Busca un artículo por su ID y devuelve el número de fila en el que está almacenado, funciona de manera similar al buscar usuario
        static int BuscarArticulo(string Identificacion)
        {
            for (int i = 0; i < totalArticulos; i++)
            {
                if (articulos[i, ID] == Identificacion)
                {
                    return i;
                }
            }
            return -1;
        }
        //Se usa para tomar un dato de la matriz de los artículos expresada en string y convertirla a int, el pos indica la posición osea el artículo, y el dato es lo que se quiere pasar a int, si el precio o el stock
        static int DevuelveInt(int pos, int dato)
        {
            return int.Parse(articulos[pos, dato]);
        }

        static void ListaUsuarios()
        {
            Console.WriteLine("\n---------- LISTA DE USUARIOS ----------");
            if (totalUsuarios == 0)//En caso de no haber usuarios registrados, avisa y termina el módulo
            {
                Console.WriteLine("No hay usuarios registrados");
                return;
            }

            for (int i = 0; i < totalUsuarios; i++)//Registra solo las filas ocupadas
            {
                Console.WriteLine($"{i + 1}. C.C: {usuarios[i, CC]} | Nombre: {usuarios[i, Nombre]} | Teléfono {usuarios[i, Telefono]}");
            }
        }
        //Detiene el programa hasta que el usuario presione una tecla, para que alcance a leer el resultado antes de limpiar la pantalla
        static void Pausa()
        {
            Console.WriteLine("\nPresione una tecla para continuar...");
            Console.ReadKey();
        }
        static void NuevoUsuario()
        {
            Console.WriteLine("\n---------- NUEVO USUARIO ----------");
            if (totalUsuarios == MaxUsuarios)//Verifica si se pueden seguir registrando usuarios, en caso de que no, avisa y termina el módulo
            {
                Console.WriteLine("ERROR: Se alcanzó el máximo de 20 usuarios");
                return;
            }
            string cedula = CapturarTexto("Cédula: ");//Primero pide la cédula, para verificar que no hayan más usuarios registrados con la misma
            if (BuscarUsuario(cedula) != -1)//Si el módulo BuscarUsuario devuelve un valor distinto a -1 es porque encontró un usuario ya registrado con esa cédula
            {
                Console.WriteLine("ERROR: Ya existe un usuario con esa cédula");
                return;
            }
            //Pasada la verificación, ahora si guarda un nuevo usuario en la primera fila disponible
            usuarios[totalUsuarios, CC] = cedula;
            usuarios[totalUsuarios, Nombre] = CapturarTexto("Nombre: ");
            usuarios[totalUsuarios, Telefono] = CapturarTexto("Teléfono: ");
            totalUsuarios++;//Se incrementa después de guardar, para que no se registren los datos en la fila equivocada

            Console.WriteLine("Usuario registrado correctamente");
        }
        //Edita el nombre y el teléfono de un usuario existente
        static void EditarUsuario()
        {
            Console.WriteLine("\n---------- EDITAR USUARIO ----------");
            string cedula = CapturarTexto("Ingrese la cédula del usuario a editar: ");

            int pos = BuscarUsuario(cedula);//Pregunta por la cédula del usuario a editar para saber la fila en la que sobreescribirá los datos
            if (pos == -1)
            {
                Console.WriteLine("ERROR: No existe un usuario con esa cédula");
                return;
            }
            Console.WriteLine($"Editando a {usuarios[pos, Nombre]} (la cédula no se modifica).");//pos=posición del usuario en la matriz (la fila en la que está ubicado)
            usuarios[pos, Nombre] = CapturarTexto("Nuevo nombre: ");
            usuarios[pos, Telefono] = CapturarTexto("Nuevo teléfono: ");

            Console.WriteLine("Usuario actualizado correctamente");
        }
        static void Articulos()
        {
            int opcion;
            do
            {
                Console.Clear();
                Console.WriteLine("\n---------- MENÚ ARTÍCULOS ----------");
                Console.WriteLine("1. Mostrar lista de artículos");
                Console.WriteLine("2. Nuevo Artículo");
                Console.WriteLine("3. Editar Artículo");
                Console.WriteLine("4. Regresar al menú principal");
                Console.WriteLine("-------------------------------------");
                Console.WriteLine("Ingrese que operación desea realizar:");
                opcion = CapturarOpción(1, 4);
                switch (opcion)
                {
                    case 1:
                        ListaArticulos();
                        Pausa();
                        break;
                    case 2:
                        NuevoArticulo();
                        Pausa();
                        break;
                    case 3:
                        EditarArticulo();
                        Pausa();
                        break;
                }
            } while (opcion != 4);
        }
        static void ListaArticulos()
        {
            Console.WriteLine("\n--------- LISTA DE ARTÍCULOS ----------");
            if (totalArticulos == 0)//En caso de no haber artículos registrados, avisa y termina el módulo
            {
                Console.WriteLine("No hay artículos disponibles");
                return;
            }

            for (int i = 0; i < totalArticulos; i++)//Registra solo las filas ocupadas
            {
                Console.WriteLine($"{i + 1}. I.D: {articulos[i, ID]} | Nombre: {articulos[i, Nombre]} | Stock {articulos[i, Stock]} | Precio COL${articulos[i, Precio]}");
            }
        }

        static void NuevoArticulo()
        {
            Console.WriteLine("\n---------- NUEVO ARTÍCULO ----------");
            if (totalArticulos == MaxArticulos)//Verifica si se pueden seguir registrando artículos, en caso de que no, avisa y termina el módulo
            {
                Console.WriteLine("ERROR: Se alcanzó el máximo de 20 artículos");
                return;
            }
            string Identificacion = CapturarTexto("ID: ");//Primero pide el ID, para verificar que no hayan más artículos registrados con la misma
            if (BuscarArticulo(Identificacion) != -1)//Si el módulo BuscarArticulo devuelve un valor distinto a -1 es porque encontró un artículo ya registrado con ese ID
            {
                Console.WriteLine("ERROR: Ya existe un artículo con ese ID");
                return;
            }
            //Pasada la verificación, ahora si guarda un nuevo artículo en la primera fila disponible
            articulos[totalArticulos, ID] = Identificacion;
            articulos[totalArticulos, Nombre] = CapturarTexto("Nombre: ");
            articulos[totalArticulos, Stock] = CapturarNumero("Stock: ", 0).ToString(); //Samuel Arango añadió método ToString
            articulos[totalArticulos, Precio] = CapturarNumero("Precio: ", 1).ToString(); //Samuel Arango añadió método ToString
            totalArticulos++;//Se incrementa después de guardar, para que no se registren los datos en la fila equivocada

            Console.WriteLine("Artículo registrado correctamente");
        }

        static void EditarArticulo()
        {
            Console.WriteLine("\n---------- EDITAR ARTÍCULO ----------");
            string Identificacion = CapturarTexto("Ingrese el ID del artículo a editar: ");
            int pos = BuscarArticulo(Identificacion);//Pregunta por el ID del artículo a editar para saber la fila en la que sobreescribirá los datos
            if (pos == -1)
            {
                Console.WriteLine("ERROR: No existe un artículo con ese ID");
                return;
            }
            Console.WriteLine($"Editando a {articulos[pos, Nombre]} (El ID no se modifica).");//pos=posición del artículo en la matriz (la fila en la que está ubicado)
            articulos[pos, Nombre] = CapturarTexto("Nuevo nombre: ");
            articulos[pos, Stock] = CapturarNumero("Nuevo Stock: ", 0).ToString(); //Samuel Arango añadió método ToString
            articulos[pos, Precio] = CapturarNumero("Nuevo Precio: ", 1).ToString(); //Samuel Arango añadió método ToString
            Console.WriteLine("Artículo actualizado correctamente"); 
        }

        static void Venta()
        {
            Console.Clear();
            Console.WriteLine("\n---------- VENTA ----------");
            if (totalArticulos == 0)//Sin artículos no se puede vender
            {
                Console.WriteLine("ERROR: No hay artículos registrados");
                return;
            }
            string cedula = CapturarTexto("Cédula del comprador:");
            int Usuario = BuscarUsuario(cedula);//Se guarda la fila para mostrar el nombre en la factura
            if (Usuario == -1)
            {
                Console.WriteLine("ERROR: No se encontró ningún usuario con esa cédula");
                NuevoUsuario();
                return;
            }

            //Empieza a añadir artículos

            int[] posCompra = new int[MaxArticulos];//Fila de cada artículo comprado, osea guarda la posición del artículo en la matriz artículos.
            int[] cantCompra = new int[MaxArticulos];//Cantidad comprada de cada uno, guarda en el mismo índice de posCompra, pero guarda cantidad, no posición.
            int itemsCompra = 0;//Líneas distintas en la compra
            string seguir;
            do
            {
                Console.WriteLine();
                ListaArticulos();
                int pos = PedirArticuloValido();//Pide artículo para obtener su posición.
                int stock = DevuelveInt(pos, Stock);//Convierte el stock de esa posición en un entero para poder operar con él.

                if (stock == 0)
                {
                    Console.WriteLine("ERROR: Ese artículo no tiene stock disponible");
                }
                else//Sólo se ejecuta si hay stock
                {
                    int cantidad = CapturarNumero("Cantidad a comprar: ", 1);
                    if (cantidad > stock)//No deja comprar si se pide más cantidad que stock.
                    {
                        Console.WriteLine($"ERROR: Solo hay {stock} unidades disponibles");//Muestra stock disponible
                    }
                    else//Se ejecuta cuando la cantidad cabe en el stock.
                    {
                        articulos[pos, Stock] = (stock - cantidad).ToString(); //Resta del stock, convierte el número a string.

                        int linea = BuscarEnCompra(posCompra, itemsCompra, pos);
                        if (linea == -1) //Artículo nuevo en esta compra
                        {
                            posCompra[itemsCompra] = pos;
                            cantCompra[itemsCompra] = cantidad;
                            itemsCompra++;
                        }
                        else //Si el artículo ya existe, solo se suma a la cantidad del artículo, sin añadir items y manteniendo su posición.
                        {
                            cantCompra[linea] += cantidad;
                        }
                        Console.WriteLine("Artículo agregado a la compra");
                    }
                }

                do
                {
                    Console.Write("¿Desea agregar otro artículo? (S/N): ");
                    seguir = Console.ReadLine().ToUpper();//ToUpper para que se pueda ingresar s o n en minúsculas
                } while (seguir != "S" && seguir != "N");

            } while (seguir == "S");

            if (itemsCompra == 0)//Si no hay items, osea líneas válidas, no hay factura.
            {
                Console.WriteLine("No se agregó ningún artículo, no se genera factura");
                return;
            }
            GenerarFactura(cedula, posCompra, cantCompra, itemsCompra);//Se llaman las variables especíificas de la compra, por lo que ya se distingue la cedula del comprador del resto
        }

        static int PedirArticuloValido()//Se llama para evaluar cada artículo y ver si existe.
        {
            int pos;
            do
            {
                string id = CapturarTexto("ID del artículo: ");
                pos = BuscarArticulo(id);
                if (pos == -1)
                {
                    Console.WriteLine("ERROR: No existe un artículo con ese ID, intente de nuevo");
                }
            } while (pos == -1);//El módulo se ejecuta una vez, y luego se evalúa si la posición es -1, si es -1, el módulo se repite hasta cambiar.
            return pos;
        }

        //Busca si un artículo ya está en la compra; devuelve su línea o -1
        static int BuscarEnCompra(int[] posCompra, int items, int pos)
        {
            for (int i = 0; i < items; i++)
            {
                if (posCompra[i] == pos)
                {
                    return i;
                }
            }
            return -1;
        }

        static void GenerarFactura(string cedula, int[] posCompra, int[] cantCompra, int items)//No encontré forma de disminuir el número de parámetros.
        {
            int total = 0;
            Console.WriteLine("\n=============== FACTURA ===============");
            Console.WriteLine($"C.C. comprador: {cedula}");
            Console.WriteLine($"Nombre: {usuarios[BuscarUsuario(cedula), Nombre]}");//Solo se necesita el nombre del arrray de usuarios, no es necesario usar un array como posCompra porque solo se muestra.
            Console.WriteLine("---------------------------------------");
            for (int i = 0; i < items; i++)//Se ejecuta por cada item comprado
            {
                int precio = DevuelveInt(posCompra[i], Precio);
                int subtotal = precio * cantCompra[i];
                total += subtotal;
                Console.WriteLine($"{articulos[posCompra[i], Nombre]} | Cant: {cantCompra[i]} | Precio: COL${precio} | Subtotal: COL${subtotal}");
            }
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"TOTAL A PAGAR: COL${total}");
            Console.WriteLine("=======================================");
        }
    }
}