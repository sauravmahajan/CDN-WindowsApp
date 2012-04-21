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
        
    	/*try{
    		FileInputStream configFile =new FileInputStream(new File("config/MServer.cfg"));
    		new AppConfig();
    		AppConfig.load(configFile);
    		configFile.close();
    	}
    	catch(IOException e){
    		System.out.println("Config file reading in MServer failed");
    		return;
    	}*/
		
		
    	//int port = Integer.parseInt(AppConfig.getProperty("MServer.Port"));
        int port =5010;
        //TODO:read from config file
        /*if (args.length>=1)
        {
            port = Integer.parseInt(args[0]);
        }*/
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

        in_data = inp.readLine();
        //out.close();
        //in_data = inp.readLine();
        while (!in_data.equalsIgnoreCase("null")) {
            out.write(Integer.parseInt(in_data));
            System.out.println(in_data);
            in_data = inp.readLine();
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

    OutputStream t_out = connection.getOutputStream();
    BufferedWriter out2 = new BufferedWriter(new OutputStreamWriter(t_out));

    int i =0;
    while(toBeSend!=-1){
        out2.write(""+toBeSend+'\n');
        //send(""+toBeSend+'\n');
    	//System.out.println("Sending String :"+toBeSend);
    	toBeSend = file.read();
        if( i%128==0){
            out2.flush();
        }
        i++;
    }
    out2.flush();
    /*
    while(toBeSend!=null){
    	send(""+toBeSend+'\n');
    	System.out.println("Sending String :"+toBeSend);
    	toBeSend = out.readLine();
    }*/
    out2.write("null\n");
    out2.flush();
    //send("null\n");
    file.close();
    System.out.println("file sent");
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
