WeekTWeets
==========

(ES) Permite programar el envió de Tweets repetitivos semanalmente.
(EN) Program allows sending Tweets repetitive weekly.

Versión / Versions
-------------------

** (ES) Versión Actual es 1.0.0 **<br>
** (EN) Current version is 1.0.0 **<br>


Contributing / Contribución
---------------------------

(ES) Aceptamos cualquier contribución o bugs detectados.<br>
(EN) We welcome contributions, be it bug fixes or other improvements. If you fix or change something, please submit a pull request. If you want to report a bug, please open an issue. <br>


Uso / How To
============


(ES) Lo primero es obtener las "Keys" y "Access Tokens" de tu cuenta de Twitter desde https://apps.twitter.com/ y configurar twitter_cfg.json<br>
(EN) First, needs to retrieve the "Keys And Access Tokens" of your Twitter account, from https://apps.twitter.com/ and configure twitter_cfg.json<br>
<br>
Ejemplo / Example : twitter_cfg.json

```Json
{
  "AccessToken" : "444516776----myaccesstoken--9B3SZ4uLK6qD7VC8",
  "AccessSecret" : "Wg1gzUHX----myaccesssecret--dl3k$dkfrtTZ0",
  "ConsumerKey" : "jNtmt--myconsumerkey--SGafoU30u4z5",
  "ConsumerSecret" : "edBmc--myconsumersecret--aX3hYk66IvaaJ0yyM3ii"
}
```

(ES) Seguidamente es necesario configurar las tareas programadas en schedule.json<br>
(EN) Then you need to configure scheduled tasks in schedule.json<br>

(ES)Campos:<br>
   __ dayweek -> Puede especificar las iniciales de los días de la semana que se va a ejecutar "LMXJVSD".<br>
   __ time    -> Hora de ejecución ("HH:MM:SS")<br>
   __ msg     -> Tweet a enviar (máximo 140 caracteres).<br>
   __ last    -> Especifica la última vez que se ha ejecutado, por lo que no se ejecutará más en esa fecha. (DD/MM/AA)<br>
   __ count   -> Número de veces que se va a ejecutar. Para que se ejecute de forma indefinida hay que indicar "-1"<br>
   <br>
(EN)Fields:<br>
   __ dayweek -> Use the initials of Spanish day week to schedule the tweets "LMXJVSD".<br>
   __ time    -> Start time ("HH:MM:SS")<br>
   __ msg     -> Text to send (max 140 chars).<br>
   __ last    -> Specifies the last executed date, so it will not run over on that date. (DD/MM/YY)<br>
   __ count   -> Repeat Counter. "-1" For infinite<br>
<br>
Ejemplo / Example : twitter_cfg.json

```Json
{
  "msgs": [
    {
      "dayweek": "LJ",
      "time": "9:00:00",
      "msg": "Enviar Tweet 4 veces, los lunes y jueves a las 9H | Send Tweet 4 times, Monday and Thursday at 9 am",
      "last": "01/01/16",
      "count": "4"
    },
    {
      "dayweek": "V",
      "time": "17:00:00",
      "msg": "Enviar sin límite, cada Viernes a las 17h | Send unlimited , every Friday at 17h",
      "last": "17/01/16",
      "count": "-1"
    }
  ]
}

```

(ES) Usar el "Programador de tareas de Windows" para planificar las ejecuciones de WeekTWeets.<br>
(EN) Use the "windows scheduler" to plan the WeekTWeets executions.<br>

Referencias / References
========================

NewtonSoft  : http://www.newtonsoft.com/json/help/html/Introduction.htm <br>
TinyTwitter : https://github.com/jmhdez/TinyTwitter <br>
 

Licencia / Licence
==================

This library is released under the Apache 2.0 licence. See the LICENSE.md file