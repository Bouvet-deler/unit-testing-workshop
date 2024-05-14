package Webshop.Service.Common;

import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;

public class Logger {

    public static void logError(String message) {
        log("Error", message);
    }

    public static void logWarning(String message) {
        log("Warning", message);
    }

    public static void logInfo(String message) {
        log("Info", message);
    }

    private static void log(String level, String message) {
        String logFilePath = "\\\\someserversomewhere\\log.txt";
        try (PrintWriter writer = new PrintWriter(new FileWriter(logFilePath, true))) {
            String formattedDateTime = LocalDateTime.now().format(DateTimeFormatter.ISO_LOCAL_DATE_TIME);
            writer.println("[" + level + "] [" + formattedDateTime + "] " + message);
        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
