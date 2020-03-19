(ns ch2.quote
  (:require [clojure.string :as string]))

;;; Input
(defn prompt
  [question]
  (print (str question " "))
  (flush)
  (-> (read-line)
      (string/trim)))

;;; Output
(defn output
  [quotes]
  (doseq [{author :author quote :quote} quotes]
    (println (str author " says, " "\"" quote "\"" "\n"))))

;;; Execute

(defn cli []
  (-> (assoc {} :quote (prompt "What is the quote?"))
      (assoc :author (prompt "Who said it?"))
      (vector)
      (output)))

(defn multiple-quotes []
  (output [{:quote "Dude, where's my car?"
            :author "Ashton Kutcher"}
           {:quote "These aren't the droids you're looking for."
            :author "Obi-Wan Kenobi"}]))

(defn main [& args]
  (if (= "cli" (first args))
    (cli)
    (multiple-quotes)))

(apply main *command-line-args*)
