
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.Random;

/**
 * This class implements threaded server. A thread is spawned for each
 * connection to the server.
 * <p>
 * Call start to accept connections on p_port.
 * @author saurav
 */
public class server {

    ServerSocket m_server;
    int m_port = 5010;

    /**
     * 
     * @param p_port port on which server runs
     */
    public server(int p_port) {
        if (p_port != -1) {
            m_port = p_port;    //by default port is 5010
        }
    }

    /**
     * starts the server socket at port m_port
     * @throws IOException
     */
    public void start() throws IOException {
        try {
            m_server = new ServerSocket(m_port);
            System.out.println("server started at port: " + m_port);
        } catch (IOException ex) {
            System.out.println("server not started at port: " + m_port);
        }

        while (true) {
            Socket s = m_server.accept();
            th_server t_server = new th_server(s);
            t_server.start();
        }
    }
}

/**
 * a threaded server implementation
 * @author saurav
 */
class th_server extends Thread {

    Socket connection;

    public th_server(Socket p_connection) {
        connection = p_connection;
    }

    /**
     * prints [thread name]: str on console
     * @param str
     */
    public void print(String str) {
        System.out.println(this.getName() + ": " + str);
    }

    public void receiveFile() throws IOException {
        InputStream t_inp = connection.getInputStream();
        BufferedReader inp = new BufferedReader(new InputStreamReader(t_inp));
        String in_data = inp.readLine();
        FileWriter out = new FileWriter(in_data);
        boolean done = false;
        int i = 0;

        while (!done) {
            int t_char;
            try {
                if ((t_char = inp.read()) == 10) {
                    done = true;
                } else {
                    //print("" + t_char);
                    out.write(t_char);
                    i++;
                }
            } catch (IOException ex) {
                print("Failed to get InputStream");
                out.close();
                break;
            }
           // print("final"+t_char);
        }
        if (done == true) {
            out.close();
        }
    }

    @Override           // TODO:all code to be written in run
    public void run() {
        print("connected to "
                + connection.getInetAddress() + " at port "
                + connection.getPort());
        //InputStream inp;
        try {
           // receiveFile();
            InputStream t_inp = connection.getInputStream();
            BufferedReader inp = new BufferedReader(new InputStreamReader(t_inp));
            String username= inp.readLine();
            //int i=0;
            //while(true){
            //  System.out.println("got i ="+i);
            //i=i+1;
            //int in_data = t_inp.read();
            //if(in_data==0)
            //  break;
            print("" + username);
            String passwrd=inp.readLine();

            print(""+passwrd);
            //}
            //} catch (IOException ex) {
            //  print("Failed to get InputStream");
            //}
            //try {
            OutputStream t_out = connection.getOutputStream();
            BufferedWriter out = new BufferedWriter(new OutputStreamWriter(t_out));
            String out_data = "done";
            out.write(out_data);
            out.flush();
        } catch (IOException ex) {
            print("Failed to get InputStream");
        }

    }
}
