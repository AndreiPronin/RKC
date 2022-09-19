Set http = CreateObject("MSXML2.ServerXMLHTTP.6.0")
http.open "GET", "http://10.10.10.15:8000/api/apijob/1", FALSE
http.send
set http = nothing