using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO.Ports;

public class SMSCOMMS
{
    SerialPort serialPort;

    //Initialize the Port
    public SMSCOMMS(string comPort)
    {
        this.serialPort = new SerialPort(); 
        this.serialPort.PortName = comPort;
        this.serialPort.BaudRate = 9600;
        this.serialPort.Parity = Parity.None;
        this.serialPort.DataBits = 8;iji
        this.serialPort.StopBits = StopBits.One;
        this.serialPort.Handshake = Handshake.RequestToSend;
        this.serialPort.DtrEnable = true;
        this.serialPort.RtsEnable = true;
        this.serialPort.NewLine = System.Environment.NewLine;
    }

    //create and send SMS 
    public bool sendSMS(string cellNo, string sms)
    {
        string messages = null;
        messages = sms;
        if (this.serialPort.IsOpen == true)
        {
            try
            {
                this.serialPort.WriteLine("AT" + (char)(13));
                Thread.Sleep(4);
                this.serialPort.WriteLine("AT+CMGF=1" + (char)(13));
                Thread.Sleep(5);
                this.serialPort.WriteLine("AT+CMGS=\"" + cellNo + "\"");
                Thread.Sleep(10);
                this.serialPort.WriteLine("" + messages + (char)(26));
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return true;
        }
        else return false;
    }

    public void Opens()
    {
        try
        {
            if (this.serialPort.IsOpen == false)
            {
                this.serialPort.Open();
            }
        }
        catch (Exception ex)
        {
            ex.ToString();
        }


    }






    public String SendSMS(string phoneNo, string message)
    {
        System.IO.Ports.SerialPort port = new System.IO.Ports.SerialPort("COM1", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
        try
        {
            port.Open();
            port.Write("AT\r\n");
            System.Threading.Thread.Sleep(1000);
            port.WriteLine("AT+CMGF=1\r\n");
            System.Threading.Thread.Sleep(1000);
            port.WriteLine("AT+CMGS=\"+60121212121\"\r\n");
            port.WriteLine("AT+CMGS=\"" + phoneNo + "\"\r\n");
            System.Threading.Thread.Sleep(1000);
            port.WriteLine("Testing SMS\r\n" + '\x001a');
            port.WriteLine(message + "\r\n" + '\x001a');
            return "done";
        }
        catch(Exception e)
        {
            return e.Message;
        }
        
    }






}