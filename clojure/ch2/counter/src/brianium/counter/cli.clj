(ns brianium.counter.cli
  (:require [clojure.string :as string]))

(defn- prompt
  [question]
  (print (str question " "))
  (flush)
  (-> (read-line)
      (string/trim)))

(defn run []
  (loop [input (prompt "What is the input?")]
    (if (seq input)
      (println (str input " has " (count input) " characters"))
      (recur (prompt "Input cannot be empty. What is the input?")))))
