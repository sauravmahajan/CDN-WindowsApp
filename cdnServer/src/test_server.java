
import java.io.IOException;

/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author saurav
 */
public class test_server {

    public static void main(String [] args) throws IOException{
        int port = -1;
        if (args.length>=1)
        {
            port = Integer.parseInt(args[0]);
        }
        server myServer = new server(port);
        myServer.start();

    }

}
