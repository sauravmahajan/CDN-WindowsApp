//package prototype.user;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.OutputStreamWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.rmi.NotBoundException;
import java.rmi.registry.LocateRegistry;
import java.rmi.registry.Registry;

//import prototype.user.IUser;

//import AbstractAppConfig.AppConfig;

/**
 * This class implements threaded server. A thread is spawned for each
 * connection to the server.
 * <p>
 * Call start to accept connections on p_port.
 * @author saurav
 */
public class MServer {

	ServerSocket m_server;
    private int m_port = 5010;

    /**
     * 
     * @param p_port port on which server runs
     */
    public MServer(int p_port){
    	m_port = p_port;    //by default port is 5010
    }

    /**
     * starts the server socket at port m_port
     * @throws IOException
     */
    public void start() throws IOException{
        try {
            m_server = new ServerSocket(m_port);
            System.out.println("server: "+m_server.getInetAddress()+" started at port: " +m_port);
            
        } catch (IOException ex) {
            System.out.println("server not started at port: " +m_port);
        }

        while (true) {
            Socket s = m_server.accept();
            TServer t_server = new TServer(s);
            t_server.start();
        }
    }
    public static void main(String [] args) throws IOException{
   
        int port =5010;
        MServer myServer = new MServer(port);
        myServer.start();
    }
}

/**
 * a threaded server implementation
 * @author saurav
 */
class TServer extends Thread{

    Socket connection;
    public TServer(Socket p_connection){
        connection = p_connection;
    }
    /**
     * prints [thread name]: str on console
     * @param str
     */
    public void print(String str) {
        System.out.println(this.getName() + ": " + str);
    }
    /*
     * Receives a file
     */
    public void receiveFile() throws IOException {
        InputStream t_inp = connection.getInputStream();
        BufferedReader inp = new BufferedReader(new InputStreamReader(t_inp));
        String in_data = inp.readLine();
        FileOutputStream out = new FileOutputStream(new File(in_data));

        boolean done = false;
        while (!done) {
            in_data = inp.readLine();
            String[] all = in_data.split("\n");
            for (int i = 0; i < all.length; i++) {
                if (!all[i].equalsIgnoreCase("null")) {
                    if (!all[i].equalsIgnoreCase("")) {
                        out.write(Integer.parseInt(all[i]));
                    }
                } else {
                    done = true;
                    break;
                }
            }
        }
        out.close();
    }
    
    public boolean login() throws IOException{
    	InputStream t_inp = connection.getInputStream();
        BufferedReader inp = new BufferedReader(new InputStreamReader(t_inp));
        String username = inp.readLine();
        System.out.println("username: "+username);
        String password = inp.readLine();
        System.out.println("password: "+password);
        if(true){
        	return true;
        }
/*        try{
        Registry registry = LocateRegistry.getRegistry("localhost");
		IUser stub = (IUser) registry.lookup("userdaemon");
		stub.login(username,password);
		}
        catch(NotBoundException e){
        	return false;
        }*/
		return true;
    }

public void send(String out_data)throws IOException{
	OutputStream t_out = connection.getOutputStream();
    BufferedWriter out = new BufferedWriter(new OutputStreamWriter(t_out));
    out.write(out_data);
    out.flush();
}
public void receiveComment()throws IOException{
	System.out.println("Receiving comments");
	InputStream t_inp = connection.getInputStream();
    BufferedReader inp = new BufferedReader(new InputStreamReader(t_inp));
    String in_data = inp.readLine();
    while(!in_data.contains("-1")){
    	print(in_data);
    	in_data = inp.readLine();
    }
}
public void sendFile()throws IOException{
	System.out.println("IN Sending file");
	InputStream t_inp = connection.getInputStream();
    BufferedReader inp = new BufferedReader(new InputStreamReader(t_inp));
    String fileName = inp.readLine();
    System.out.println("Sending file :"+fileName);
    //BufferedReader out = new BufferedReader( new FileReader("config/"+fileName));
    //BufferedReader out = new BufferedReader( new FileReader(fileName));
    FileInputStream file = new FileInputStream(new File(fileName));

    //String toBeSend = out.readLine();
    int toBeSend = file.read();

    try{
    OutputStream t_out = connection.getOutputStream();
    BufferedWriter out2 = new BufferedWriter(new OutputStreamWriter(t_out));

    int i =0;
    while(toBeSend!=-1){
        out2.write(""+toBeSend+'\n');
        //send(""+toBeSend+'\n');
    	//System.out.println("Sending String :"+toBeSend);
    	toBeSend = file.read();
        if( i%128==0){
            //out2.flush();
        }
        i++;
    }
    out2.write("null\n");
    out2.flush();
    System.out.println("file sent");
    }
    catch(Exception e){
        System.out.println("file not sent : " +e.getMessage() );
    }
    file.close();
}
public void sendresume()throws IOException{
	System.out.println("IN resume file");
	InputStream t_inp = connection.getInputStream();
    BufferedReader inp = new BufferedReader(new InputStreamReader(t_inp));
    String fileName = inp.readLine();
    int start = Integer.parseInt(inp.readLine());
    System.out.println("Sending file :"+fileName+"from: "+ start);

    FileInputStream file = new FileInputStream(new File(fileName));


    int moved = (int)file.skip(start);
    int left = start-moved;
    
    while(left>0){
        file.read();
        left--;
    }

    int toBeSend = file.read();

    try{
    OutputStream t_out = connection.getOutputStream();
    BufferedWriter out2 = new BufferedWriter(new OutputStreamWriter(t_out));

    int i =0;
    while(toBeSend!=-1){
        out2.write(""+toBeSend+'\n');
    	toBeSend = file.read();
        if( i==524288){
            break;
        }
        i++;
    }
    if(toBeSend ==-1){
    out2.write("null\n");}
    else{out2.write("null1\n");}
    out2.flush();
    System.out.println("file sent");
    }
    catch(Exception e){
        System.out.println("file not sent : " +e.getMessage() );
    }
    file.close();
}

    @Override           // TODO:all code to be written in run
    public void run(){
        System.out.println("Thread: "+this.getName()+" connected to "+
                connection.getInetAddress()+" at port "+ connection.getPort());
        try {
            // receiveFile();
        	if(!login()){
   			 System.out.println("login in MServer failed");
   		    }
        	send("done");
        	InputStream t_inp = connection.getInputStream();
            BufferedReader inp = new BufferedReader(new InputStreamReader(t_inp));
            String in_data = inp.readLine();
             
            int response = Integer.parseInt(in_data);
            while(response!=-1){
            	System.out.println("response received :"+response);
            	 switch(response){
                    case 0: receiveComment();break;
                    case 1: sendFile();break;
                    case 2:receiveFile();break;
                    case 3: sendresume();break;
            	 }
            	 in_data = inp.readLine();
                    System.out.println(in_data);
            	 response = Integer.parseInt(in_data);
             }
            System.out.println("Terminated");
             //print("" + in_data);
             
             
         } catch (IOException ex) {
             print("Failed to get InputStream");
         }
    }
}
