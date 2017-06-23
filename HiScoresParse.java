import java.io.*;
import java.net.URL;

/**
 * Created by Sajiel on 6/23/17.
 */
public class HiScoresParse {
    public static boolean validPlayer(String player) throws IOException {
        URL liteLookup = new URL("http://services.runescape.com/m=hiscore/index_lite.ws?player=" + player);
        try {
            new BufferedReader(new InputStreamReader(liteLookup.openStream()));
        } catch (IOException e) {
            return false;
        }

        return true;
    }

   public static void getTopPlayers() throws IOException {
        // Structure for URL without page number
        String genericPageURL = "http://services.runescape.com/m=hiscore/ranking?" +
                "category_type=0&table=0&time_filter=0&date=0&page=";
        // Holds full URL for HiScores page
        String currentPage;
        // Current page
        int page;
        URL hiscores;
        BufferedReader in;
        String line;
        String player;

        page = 1;
        BufferedWriter bw = new BufferedWriter(new FileWriter("/Users/Sajiel/Downloads/RuneScape HiScores/src/names.txt"));

        do {
            currentPage = genericPageURL.concat(Integer.toString(page));
            hiscores = new URL(currentPage);
            in = new BufferedReader(new InputStreamReader(hiscores.openStream()));
            while ((line = in.readLine()) != null) {
                if (line.contains("<img class=\"avatar\" src='http://services.runescape.com/m=avatar-rs/")) {
                    player = (line.split("/")[4] + "\n").replace('+',' ');
                    bw.write(player);
                }
            }
            in.close();
            page++;

        }
        while (page < 20000);
        bw.close();
    }

    public static void main(String[] args) throws IOException {
        HiScoresParse.getTopPlayers();
    }
}
