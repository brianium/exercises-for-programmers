(ns ch2
  (:require [clojure.string :as string]))

;;; Input
(defn prompt
  [question]
  (print (str question " "))
  (flush)
  (-> (read-line)
      (string/trim)))

;;; Output
(defmulti format-message identity)
(defmethod format-message "Brian" [_] "Greetings benevolent creator!")
(defmethod format-message "Jennie" [_] "Hello Jennie! You are super rad!")
(defmethod format-message :default [input] (str "Hello, " input ", nice to meet you!"))

(defn output-message
  [msg]
  (println msg))

;;; Execute
(defn main []
  (-> (prompt "What is your name?")
      (format-message)
      (output-message)))

(main)
